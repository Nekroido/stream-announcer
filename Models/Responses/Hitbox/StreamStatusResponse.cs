using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Announcer.Models.Responses.Hitbox
{
    public class StreamStatusResponse
    {
        [JsonProperty("livestream")]
        public List<StreamResponse> Streams { get; set; }
    }
}
