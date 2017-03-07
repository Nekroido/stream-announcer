using Announcer.Classes.Helpers;
using Announcer.Interfaces;
using Announcer.Models.Helpers;
using Announcer.Models.Payloads;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Threading.Tasks;

namespace Announcer.Classes.Services
{
    public class DiscordService : ApiBase, IAnnounceService
    {
        const string WEBHOOK_URL = "https://discordapp.com/api/webhooks/{0}/{1}";

        private AnnounceConfig config;

        public DiscordService(AnnounceConfig config)
        {
            this.config = config;
            this.config.ConfigOverride = config.ConfigOverride ?? Config.GetServiceConfig(AnnounceType.discord.ToString());
        }

        public async Task<uint> Announce(MessagePayload payload)
        {
            if (config.UseRandomPoster && string.IsNullOrEmpty(payload.PosterPath))
                payload.PosterPath = FileService.GetRandomPoster();

            return await Announce($"{this.config.Message.Prepend}{payload.Message}{this.config.Message.Append}", payload.PosterPath);
        }

        public async Task<uint> Announce(string message, string posterPath)
        {
            var url = string.Format(WEBHOOK_URL, config.ConfigOverride.Id, config.ConfigOverride.Token);
            var response = await PostRequest(url, new Dictionary<string, object>
            {
                { "content", message }
            }, new NameValueCollection
            {
                { "file", posterPath }
            });

            return 0;
        }
    }
}
