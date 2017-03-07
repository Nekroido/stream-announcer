using Announcer.Classes.Helpers;
using Announcer.Interfaces;
using Announcer.Models;
using Announcer.Models.Helpers;
using Announcer.Models.Responses.GoodGame;
using System.Net.Http;
using System.Threading.Tasks;

namespace Announcer.Classes.Services
{
    public class GoodGameService : ApiBase, IStreamInfoService
    {
        const string API_URL = "https://api2.goodgame.ru/streams/{0}";

        private StreamConfig config;

        public GoodGameService(StreamConfig config)
        {
            this.config = config;
        }

        public override HttpClient GetClient()
        {
            var client = base.GetClient();

            client.DefaultRequestHeaders.Add("Accept", "application/json");

            return client;
        }

        public async Task<ChannelInfo> GetStreamInfoAsync(string channel)
        {
            var status = await GetRequest<StatusResponse>(string.Format(API_URL, channel), null);

            if (status?.Channel == null)
            {
                return new ChannelInfo
                {
                    Channel = channel
                };
            }
            else
            {
                return new ChannelInfo
                {
                    Channel = channel,
                    Title = status.Channel.Status,
                    Media = status.Channel.Game?.Title,
                    Viewers = status.Viewers,
                    IsLive = status.IsLive,
                    Success = true
                };
            }
        }
    }
}
