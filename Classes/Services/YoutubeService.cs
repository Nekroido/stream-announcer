using Announcer.Classes.Helpers;
using Announcer.Interfaces;
using Announcer.Models;
using Announcer.Models.Helpers;
using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Collections.Specialized;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Announcer.Classes.Services
{
    public class YoutubeService : ApiBase, IStreamInfoService
    {
        const string CHANNEL_URL = "https://www.youtube.com/channel/{0}/live";
        const string VIEWERS_URL = "https://www.youtube.com/live_stats";
        const string INFO_URL = "https://www.youtube.com/get_video_info";
        private StreamConfig config;

        public YoutubeService(StreamConfig config)
        {
            this.config = config;
        }

        public async Task<ChannelInfo> GetStreamInfoAsync(string channel)
        {
            var embedKey = await getEmbedKey(channel);

            if (string.IsNullOrEmpty(embedKey) == false)
            {
                var viewersString = await GetRequest(VIEWERS_URL, new NameValueCollection
                {
                    { "v", embedKey }
                });
                UInt32.TryParse(viewersString, out uint viewers);

                var info = QueryHelpers.ParseQuery(await GetRequest(INFO_URL, new NameValueCollection
                {
                    { "video_id", embedKey }
                }));

                return new ChannelInfo
                {
                    Channel = channel,
                    Title = info.FirstOrDefault(x => x.Key == "title").Value.ToString() ?? "",
                    Media = info.FirstOrDefault(x => x.Key == "title").Value.ToString() ?? "",
                    Viewers = viewers,
                    IsLive = info.Any(x => x.Key == "live_chunk_readahead"),
                    Success = true
                };
            }
            else
            {
                return new ChannelInfo
                {
                    Channel = channel
                };
            }
        }

        private async Task<string> getEmbedKey(string channel)
        {
            var page = await GetRequest(string.Format(CHANNEL_URL, channel), null);

            var match = Regex.Match(page, @"youtube\.com\/v\/([\w-]+)");

            return match.Success ? match.Groups[1].Value : "";
        }
    }
}
