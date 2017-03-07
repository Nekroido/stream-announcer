using Newtonsoft.Json;

namespace Announcer.Models.Responses.Bargonia
{
    public class DetailsResponse
    {
        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("vie")]
        private int vie { get; set; }

        [JsonProperty("game")]
        public string Game { get; set; }

        public uint Viewers
        {
            get { return (uint)(vie > 0 ? vie : 0); }
        }

        public bool IsLive
        {
            get { return vie >= 0; }
        }
    }
}
