using Announcer.Classes.Converters;
using Newtonsoft.Json;

namespace Announcer.Models.Responses.CyberGame
{
    public class StatusResponse
    {
        public string Status { get; set; } = "";
        
        public string Game { get; set; } = "";

        [JsonProperty("spectators")]
        public uint Viewers { get; set; } = 0;

        [JsonProperty("online")]
        [JsonConverter(typeof(JsonBoolConverter))]
        public bool IsLive { get; set; } = false;
    }
}
