using Announcer.Classes.Helpers;
using Announcer.Interfaces;
using Announcer.Models;
using Announcer.Models.Helpers;
using Announcer.Models.Responses.CyberGame;
using System.Collections.Specialized;
using System.Threading.Tasks;

namespace Announcer.Classes.Services
{
    public class CyberGameService : ApiBase, IStreamInfoService
    {
        const string API_URL = "http://api.cybergame.tv/p/statusv2/";

        private StreamConfig config;

        public CyberGameService(StreamConfig config)
        {
            this.config = config;
        }

        public async Task<ChannelInfo> GetStreamInfoAsync(string channel)
        {
            StatusResponse status;
            try
            {
                status = await GetRequest<StatusResponse>(string.Format(API_URL, channel), new NameValueCollection
                {
                    { "channel", channel }
                });
            }
            catch
            {
                status = null;
            }

            if (status == null)
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
                    Media = status.Game,
                    Viewers = status.Viewers,
                    IsLive = status.IsLive,
                    Success = true
                };
            }
        }
    }
}
