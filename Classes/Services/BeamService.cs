using Announcer.Classes.Helpers;
using Announcer.Interfaces;
using Announcer.Models;
using Announcer.Models.Helpers;
using Announcer.Models.Responses.Beam;
using System.Threading.Tasks;

namespace Announcer.Classes.Services
{
    public class BeamService : ApiBase, IStreamInfoService
    {
        const string API_URL = "https://beam.pro/api/v1/channels/{0}";

        private StreamConfig config;

        public BeamService(StreamConfig config)
        {
            this.config = config;
        }

        public async Task<ChannelInfo> GetStreamInfoAsync(string channel)
        {
            var status = await GetRequest<ChannelResponse>(string.Format(API_URL, channel), null);

            if (string.IsNullOrEmpty(status?.Status))
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
                    Title = status.Status,
                    Media = status.Game?.Name,
                    Viewers = status.Viewers,
                    IsLive = status.IsLive,
                    Success = true
                };
            }
        }
    }
}
