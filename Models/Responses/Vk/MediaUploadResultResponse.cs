using Newtonsoft.Json;

namespace Announcer.Models.Responses.Vk
{
    public class MediaUploadResultResponse
    {
        [JsonProperty("server")]
        public string Server { get; set; }

        [JsonProperty("hash")]
        public string Hash { get; set; }

        [JsonProperty("photo")]
        public string Photo { get; set; }
    }
}
