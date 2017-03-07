using Newtonsoft.Json;
using System.Collections.Generic;

namespace Announcer.Models.Responses.Vk
{
    public class BaseListResponse
    {
        [JsonProperty("count")]
        public int Count { get; set; }

        [JsonProperty("items")]
        public List<object> Items { get; set; }
    }
}
