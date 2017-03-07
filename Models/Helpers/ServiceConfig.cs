using Newtonsoft.Json;

namespace Announcer.Models.Helpers
{
    public class ServiceConfig
    {
        [JsonProperty("type")]
        public ServiceType Type { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("token")]
        public string Token { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }
    }
}
