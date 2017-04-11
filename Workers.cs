using Announcer.Classes;
using Announcer.Classes.Helpers;
using Announcer.Classes.Services;
using Announcer.Models;
using Announcer.Models.Payloads;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Announcer
{
    public class Workers
    {
        private static readonly SemaphoreSlim announceSl = new SemaphoreSlim(initialCount: 1);

        private static readonly SemaphoreSlim webhookSl = new SemaphoreSlim(initialCount: 1);

        public static async Task DoChannelStatusUpdate(string channel)
        {
            var stopWatch = new Stopwatch();
            TimeSpan ts;
            while (true)
            {
                var generalConfig = Config.Load();
                uint sleepDelay = generalConfig.Delay;
                var config = generalConfig.Channels.FirstOrDefault(x => x.Channel == channel);

                if (config == null)
                    goto SKIP_TO_WAIT;

                var primaryChannel = config.Stream.FirstOrDefault(x => x.IsEnabled == true && x.IsPrimary == true);

                if (primaryChannel == null)
                {
                    Logger.Log($"No primary channel set for {config.Channel}. Using the first one.", Logger.Severity.error);
                    primaryChannel = config.Stream.First();
                }

                stopWatch.Start();

                var channelInfos = new List<ChannelInfo>();
                foreach (var streamConfig in config.Stream.Where(x => x.IsEnabled == true))
                {
                    var service = ServiceFactory.GetStreamInfoService(streamConfig);
                    channelInfos.Add(await service.GetStreamInfoAsync(streamConfig.Channel));
                }

                // Primary channel is inaccessible
                if (channelInfos.Exists(x => x.Channel == primaryChannel.Channel && x.Success == false))
                {
                    Logger.Log($"Primary channel is down for {config.Channel}.", Logger.Severity.error);
                    sleepDelay = config.LiveWaitDelay;
                    goto SKIP_TO_WAIT;
                }

                var dashboard = new Dashboard
                {
                    ChannelId = config.ChannelId,
                    Channel = channel
                };
                foreach (var channelInfo in channelInfos)
                {
                    dashboard.Title = string.IsNullOrEmpty(dashboard.Title) == false ? dashboard.Title : channelInfo.Title;
                    dashboard.Media = string.IsNullOrEmpty(dashboard.Media) == false ? dashboard.Media : channelInfo.Media;
                    dashboard.Viewers += channelInfo.Success ? channelInfo.Viewers : 0;
                    dashboard.IsLive = dashboard.IsLive || channelInfo.IsLive;
                }

                dashboard.Title = StringHelper.RemoveByArray(dashboard.Title, generalConfig.RemoveStringsFromTitle);
                dashboard.MaxViewers = dashboard.Viewers;

                var previousDashboard = Dashboard.GetPrevious(channel);
                var isPrevious = dashboard.IsPrevious(previousDashboard);

                var callWebhook = false;
                if (previousDashboard == null)
                {
                    callWebhook = true;
                    await dashboard.Save();
                }

                if (isPrevious == true && dashboard.IsLive && previousDashboard != null)
                {
                    // Additional check if stream was restarted after a delay with the same dashboard
                    var diff = DateTime.Now - previousDashboard.BroadcastEnd;
                    isPrevious = diff?.TotalMinutes >= Config.Load().StreamRestartDelay;
                }

                if (previousDashboard != null && (isPrevious == false || dashboard.Viewers != previousDashboard.Viewers || dashboard.IsLive != previousDashboard.IsLive))
                    callWebhook = true;

                if (isPrevious == false)
                {
                    dashboard.BroadcastStart = DateTime.Now;

                    if (previousDashboard != null && previousDashboard.BroadcastEnd == null)
                    {
                        previousDashboard.BroadcastEnd = DateTime.Now;
                        previousDashboard.IsLive = false;

                        await previousDashboard.Save();
                    }

                    var media = await Media.Find(config.Type, dashboard);
                    var posterPath = "";
                    if (string.IsNullOrEmpty(media?.Poster) == false)
                        posterPath = await FileService.DownloadPoster(
                            media.Poster,
                            media.Id.ToString(),
                            config.Type == ChannelType.game
                            ? FileService.PosterType.game
                            : FileService.PosterType.movie
                            );

                    if (dashboard.IsLive)
                    {
                        // Add announce message to the queue
                        Data.MessageQueue.Enqueue(new MessagePayload
                        {
                            ChannelConfig = config,
                            Message = dashboard.Title,
                            PosterPath = posterPath,
                            Dashboard = dashboard,
                            PreviousDashboard = previousDashboard
                        });
                    }
                }
                else
                {
                    dashboard.Id = previousDashboard.Id;
                    dashboard.BroadcastStart = previousDashboard.BroadcastStart;
                    dashboard.BroadcastEnd = previousDashboard.BroadcastEnd;
                    dashboard.MaxViewers = previousDashboard.MaxViewers;
                    dashboard.VkPostId = previousDashboard.VkPostId;
                }

                if (dashboard.IsLive == false)
                {
                    dashboard.BroadcastEnd = dashboard.BroadcastEnd ?? DateTime.Now;
                }
                else
                {
                    dashboard.BroadcastEnd = null;
                }

                dashboard.MaxViewers = Math.Max(dashboard.Viewers, previousDashboard?.MaxViewers == null ? 0 : previousDashboard.MaxViewers);

                await dashboard.Save();

                if (callWebhook)
                {
                    await dashboard.Save();
                    Data.WebhookQueue.Enqueue(new WebhookPayload
                    {
                        Dashboard = dashboard,
                        ChannelInfos = channelInfos
                    });
                }

                SKIP_TO_WAIT:

                stopWatch.Stop();
                ts = stopWatch.Elapsed;

                if (ts.Milliseconds < sleepDelay)
                {
                    Thread.Sleep((int)sleepDelay - ts.Milliseconds);
                }
            }
        }

        public static async Task DoDataUpdate()
        {
            while (true)
            {
                var lowestDate = DateTime.Now;
                lowestDate = lowestDate.Subtract(TimeSpan.FromDays(Config.Load().CacheUpdateDelay));
                var lastUpdate = DataUpdate.GetLastUpdate();

                if (lastUpdate == null || lastUpdate.RunDate <= lowestDate)
                {
                    Logger.Log("- Starting data update", Logger.Severity.info);
                    await GiantBombService.Instance.CachePlatforms();
                    DataUpdate.AddEntry();
                    Logger.Log("-- Data update finished", Logger.Severity.info);
                }
                Thread.Sleep(DateTime.Now - lowestDate);
            }
        }

        public static async Task DoAnnounce()
        {
            while (true)
            {
                if (Data.MessageQueue.Any())
                {
                    var payload = Data.MessageQueue.Dequeue();

                    if (payload != null)
                    {
                        foreach (var announceConfig in payload.ChannelConfig.Announce.FindAll(x => x.IsEnabled))
                        {
                            if (payload.AnnounceFor != null && payload.AnnounceFor != announceConfig.Type)
                                continue;

                            var service = ServiceFactory.GetAnnounceService(announceConfig);
                            var postId = await service.Announce(payload);

                            if (announceConfig.Type == AnnounceType.vk && postId > 0)
                            {
                                payload.Dashboard.VkPostId = postId;
                                await payload.Dashboard.Save();
                            }
                            else if (announceConfig.Type == AnnounceType.vk && postId <= 0)
                            {
                                // There was an error posting an announcement for VK
                                Logger.Log("There was an error posting an announcement for VK", Logger.Severity.error);
                                payload.AnnounceFor = AnnounceType.vk;
                                Data.MessageQueue.Enqueue(payload);
                                Thread.Sleep(5000);
                            }
                        }
                    }
                }
                else
                {
                    Thread.Sleep(5000);
                }
            }
        }

        public static async Task DoWebhookPublish()
        {
            var webhookService = new WebhookService();

            while (true)
            {
                if (Data.WebhookQueue.Any())
                {
                    var payload = Data.WebhookQueue.Dequeue();

                    if (payload != null)
                    {
                        foreach (var webhookUrl in Config.Load().StatisticsWebhooks)
                        {
                            await webhookService.Publish(payload.Dashboard, payload.ChannelInfos, webhookUrl);
                        }
                    }
                }
                else
                {
                    Thread.Sleep(5000);
                }
            }
        }
    }
}
