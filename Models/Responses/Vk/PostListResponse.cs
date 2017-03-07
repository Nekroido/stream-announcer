using Newtonsoft.Json;
using System.Collections.Generic;

namespace Announcer.Models.Responses.Vk
{
    public class PostListResponse : BaseListResponse
    {
        [JsonProperty("items")]
        public new List<Post> Items { get; set; }
        
        [JsonProperty("next_from")]
        public string NextFrom { get; set; }
    }
}
