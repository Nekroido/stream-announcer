using Newtonsoft.Json;

namespace Announcer.Models.Responses.Vk
{
    public class RequestParameter
    {
        [JsonProperty("key")]
        public string Key { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }
    }
}
