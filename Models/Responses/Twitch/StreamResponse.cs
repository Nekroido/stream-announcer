using Newtonsoft.Json;

namespace Announcer.Models.Responses.Twitch
{
    public class StreamResponse
    {
        [JsonProperty("viewers")]
        public uint Viewers { get; set; }

        [JsonProperty("channel")]
        public ChannelResponse Channel { get; set; }
    }
}
