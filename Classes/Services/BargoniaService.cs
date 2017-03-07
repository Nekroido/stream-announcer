using Announcer.Classes.Helpers;
using Announcer.Interfaces;
using Announcer.Models;
using Announcer.Models.Helpers;
using Announcer.Models.Responses.Bargonia;
using System.Threading.Tasks;

namespace Announcer.Classes.Services
{
    public class BargoniaService : ApiBase, IStreamInfoService
    {
        const string API_URL = "http://brgnchk.ru/api/cache/status_bargonia.json";

        private StreamConfig config;

        public BargoniaService(StreamConfig config)
        {
            this.config = config;
        }

        public async Task<ChannelInfo> GetStreamInfoAsync(string channel = "")
        {
            var status = await GetRequest<StatusResponse>(API_URL, null);

            if (status?.Bargonia == null)
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
                    Title = status.Bargonia.Status,
                    Media = status.Bargonia.Game,
                    Viewers = status.Bargonia.Viewers,
                    IsLive = status.Bargonia.IsLive,
                    Success = true
                };
            }
        }
    }
}
