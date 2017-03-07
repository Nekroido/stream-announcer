using Newtonsoft.Json;

namespace Announcer.Models.Responses.Twitch
{
    public class ChannelResponse
    {
        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("game")]
        public string Game { get; set; }
    }
}
