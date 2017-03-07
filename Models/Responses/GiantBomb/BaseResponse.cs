using Newtonsoft.Json;

namespace Announcer.Models.Responses.GiantBomb
{
    public class BaseResponse
    {
        [JsonProperty("error")]
        public string Status { get; set; }

        [JsonProperty("status_code")]
        public int StatusCode { get; set; }

        [JsonProperty("version")]
        public string Version { get; set; }
    }
}
