using Newtonsoft.Json;

namespace Announcer.Models.Responses.GoodGame
{
    public class ChannelResponse
    {
        [JsonProperty("title")]
        public string Status { get; set; }

        [JsonProperty("game")]
        public GameResponse Game { get; set; }
    }
}
