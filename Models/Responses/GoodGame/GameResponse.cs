using Newtonsoft.Json;

namespace Announcer.Models.Responses.GoodGame
{
    public class GameResponse
    {
        [JsonProperty("title")]
        public string Title { get; set; }
    }
}
