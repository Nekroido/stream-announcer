using Newtonsoft.Json;
using System.Collections.Generic;

namespace Announcer.Models.Responses.GiantBomb
{
    public class PlatformListResponse : BasePagedResponse
    {
        [JsonProperty("results")]
        public new List<PlatformResponse> Results { get; set; }
    }
}
