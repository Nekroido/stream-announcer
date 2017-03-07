using Newtonsoft.Json;

namespace Announcer.Models.Helpers
{
    public class AnnounceConfig
    {
        [JsonProperty("type")]
        public AnnounceType Type { get; set; }

        [JsonProperty("enabled")]
        public bool IsEnabled { get; set; } = false;

        [JsonProperty("useRandomPoster")]
        public bool UseRandomPoster { get; set; } = true;

        [JsonProperty("removeOldPosts")]
        public bool RemoveOldPosts { get; set; } = false;

        [JsonProperty("removeCommentedPosts")]
        public bool RemoveCommentedPosts { get; set; } = false;

        [JsonProperty("configOverride")]
        public ServiceConfig ConfigOverride { get; set; } = null;

        [JsonProperty("message")]
        public MessageConfig Message { get; set; } = new MessageConfig();
    }
}
