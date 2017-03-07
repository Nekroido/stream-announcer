using Newtonsoft.Json;

namespace Announcer.Models.Responses.GoodGame
{
    public class StatusResponse
    {
        [JsonProperty("is_broadcast")]
        public bool IsLive { get; set; }

        [JsonProperty("viewers")]
        public uint Viewers { get; set; }

        [JsonProperty("channel")]
        public ChannelResponse Channel { get; set; }
    }
}
