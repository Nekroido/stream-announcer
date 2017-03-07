using Announcer.Classes.Converters;
using Newtonsoft.Json;

namespace Announcer.Models.Responses.Hitbox
{
    public class StreamResponse
    {
        [JsonProperty("media_is_live")]
        [JsonConverter(typeof(JsonBoolConverter))]
        public bool IsLive { get; set; }

        [JsonProperty("media_status")]
        public string Status { get; set; }

        [JsonProperty("category_name")]
        public string Game { get; set; }

        [JsonProperty("media_views")]
        public uint Viewers { get; set; }
    }
}
