using Announcer.Classes.Helpers;
using Announcer.Interfaces;
using Announcer.Models;
using Announcer.Models.Helpers;
using Announcer.Models.Responses.Hitbox;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;

namespace Announcer.Classes.Services
{
    public class HitboxService : ApiBase, IStreamInfoService
    {
        const string API_URL = "http://api.hitbox.tv/media/live/{0}";

        private StreamConfig config;

        public HitboxService(StreamConfig config)
        {
            this.config = config;
        }

        public async Task<ChannelInfo> GetStreamInfoAsync(string channel)
        {
            var status = await GetRequest<StreamStatusResponse>(string.Format(API_URL, channel), new NameValueCollection
            {
                { "showHidden", "true" }
            });

            if (status?.Streams == null || status?.Streams.Count == 0)
            {
                return new ChannelInfo
                {
                    Channel = channel
                };
            }
            else
            {
                var info = status.Streams.First();

                if (info == null)
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
                        Title = info.Status,
                        Media = info.Game,
                        Viewers = info.Viewers,
                        IsLive = info.IsLive,
                        Success = true
                    };
                }
            }
        }
    }
}
