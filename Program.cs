using Announcer;
using Announcer.Classes.Extensions;
using Announcer.Classes.Services;
using Announcer.Db;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;

class Program
{
    static void Main(string[] args)
    {
        Logger.LogWriter = new FileService();

        using (var context = new Context())
        {
            context.Database.Migrate();
        }

        Thread threadAnnounce = null;
        Thread threadWebhook = null;
        Thread threadDataUpdate = null;

        Logger.Log("Starting Announcer", Logger.Severity.info);

        var channelThreads = new Dictionary<string, Thread>();
        while (true)
        {
            // Start service threads (restart if down)
            if (threadAnnounce == null || threadAnnounce.IsAlive == false)
            {
                Logger.Log("-- starting social announce worker", Logger.Severity.info);
                threadAnnounce = new Thread(new ThreadStart(() => Workers.DoAnnounce().Wait()));
                threadAnnounce.Start();
            }
            if (threadWebhook == null || threadWebhook.IsAlive == false)
            {
                Logger.Log("-- starting webhook worker", Logger.Severity.info);
                threadWebhook = new Thread(new ThreadStart(() => Workers.DoWebhookPublish().Wait()));
                threadWebhook.Start();
            }
            if (threadDataUpdate == null || threadDataUpdate.IsAlive == false)
            {
                Logger.Log("-- starting data update worker", Logger.Severity.info);
                threadDataUpdate = new Thread(new ThreadStart(() => Workers.DoDataUpdate().Wait()));
                threadDataUpdate.Start();
            }

            // Start channel update threads (restart if down)
            foreach (var channel in Config.Load().Channels)
            {
                if (channelThreads.ContainsKey(channel.Channel) == false || threadAnnounce.IsAlive == false)
                {
                    Logger.Log($"--- starting {channel.Channel.ToTitleCase()} channel status update worker", Logger.Severity.info);
                    channelThreads.Remove(channel.Channel);
                    channelThreads.Add(channel.Channel, new Thread(new ThreadStart(() => Workers.DoChannelStatusUpdate(channel.Channel).Wait())));
                    channelThreads[channel.Channel].Start();
                }
            }

            // Sleep for 60 seconds
            Thread.Sleep(60000);
        }
    }
}