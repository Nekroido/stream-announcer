using Newtonsoft.Json;

namespace Announcer.Models.Responses.GiantBomb
{
    public class PlatformResponse
    {
        [JsonProperty("id")]
        public uint Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("abbreviation")]
        public string Abbreviation { get; set; }
    }
}
