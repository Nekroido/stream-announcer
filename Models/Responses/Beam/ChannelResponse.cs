using Newtonsoft.Json;

namespace Announcer.Models.Responses.Beam
{
    public class ChannelResponse
    {
        [JsonProperty("online")]
        public bool IsLive { get; set; }

        [JsonProperty("name")]
        public string Status { get; set; }

        [JsonProperty("viewersCurrent")]
        public uint Viewers { get; set; }

        [JsonProperty("type")]
        public GameResponse Game { get; set; }
    }
}
