using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Announcer.Models.Responses.Vk
{
    public class PostCreatedResponse
    {
        [JsonProperty("post_id")]
        public uint PostId { get; set; }
    }
}
