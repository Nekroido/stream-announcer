using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Announcer.Models
{
    public class ChannelInfo
    {
        [JsonIgnore]
        public string Channel { get; set; }

        [JsonProperty("service")]
        [JsonConverter(typeof(StringEnumConverter))]
        public StreamType Service { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; } = "";

        [JsonProperty("media")]
        public string Media { get; set; } = "";

        [JsonProperty("viewers")]
        public uint Viewers { get; set; } = 0;

        [JsonProperty("is_live")]
        public bool IsLive { get; set; } = false;

        [JsonIgnore]
        public bool Success { get; set; } = false;
    }
}
