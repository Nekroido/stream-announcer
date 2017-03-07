using Announcer.Interfaces;
using Newtonsoft.Json;

namespace Announcer.Models.Responses.Beam
{
    public class GameResponse
    {
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
