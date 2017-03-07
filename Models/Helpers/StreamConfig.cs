using Newtonsoft.Json;

namespace Announcer.Models.Helpers
{
    public class StreamConfig
    {
        [JsonProperty("type")]
        public StreamType Type { get; set; }

        [JsonProperty("enabled")]
        public bool IsEnabled { get; set; } = false;

        [JsonProperty("primary")]
        public bool IsPrimary { get; set; } = false;

        [JsonProperty("channel")]
        public string Channel { get; set; }

        [JsonProperty("configOverride")]
        public ServiceConfig ConfigOverride { get; set; } = null;
    }
}
