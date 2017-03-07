using Announcer.Classes.Helpers;
using Announcer.Interfaces;
using Announcer.Models;
using Announcer.Models.Helpers;
using Announcer.Models.Responses.Twitch;
using System.Net.Http;
using System.Threading.Tasks;

namespace Announcer.Classes.Services
{
    public class TwitchService : ApiBase, IStreamInfoService
    {
        const string INFO_URL = "https://api.twitch.tv/kraken/channels/{0}";
        const string STATUS_URL = "https://api.twitch.tv/kraken/streams/{0}";

        private StreamConfig config;

        public TwitchService(StreamConfig config)
        {
            this.config = config;
            this.config.ConfigOverride = config.ConfigOverride ?? Config.GetServiceConfig(StreamType.twitch.ToString());
        }

        public override HttpClient GetClient()
        {
            var client = base.GetClient();

            client.DefaultRequestHeaders.Add("Accept", "application/vnd.twitchtv.v5+json");
            client.DefaultRequestHeaders.Add("Client-ID", config.ConfigOverride.Id);

            return client;
        }

        public async Task<ChannelInfo> GetStreamInfoAsync(string channel)
        {
            var status = await GetRequest<StreamStatusResponse>(string.Format(STATUS_URL, channel), null);

            if (status?.Stream != null)
            {
                return new ChannelInfo
                {
                    Channel = channel,
                    Title = status.Stream.Channel.Status,
                    Media = status.Stream.Channel.Game,
                    Viewers = status.Stream.Viewers,
                    IsLive = true,
                    Success = true
                };
            }
            else
            {
                var info = await GetRequest<ChannelResponse>(string.Format(INFO_URL, channel), null);

                return new ChannelInfo
                {
                    Channel = channel,
                    Title = info?.Status,
                    Media = info?.Game,
                    Viewers = 0,
                    IsLive = false,
                    Success = info != null
                };
            }
        }
    }
}
