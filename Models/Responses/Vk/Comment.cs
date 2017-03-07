using Newtonsoft.Json;

namespace Announcer.Models.Responses.Vk
{
    public class Comment
    {
        [JsonProperty("count")]
        public int Count { get; set; }

        [JsonProperty("can_post")]
        public bool CanPost { get; set; }
    }
}
