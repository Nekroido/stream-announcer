using Newtonsoft.Json;
using System.Collections.Generic;

namespace Announcer.Models.Responses.Tmdb
{
    public class BasePagedResponse
    {
        [JsonProperty("page")]
        public uint Page { get; set; }

        [JsonProperty("results")]
        public List<object> Results { get; set; }

        [JsonProperty("total_results")]
        public uint TotalResults { get; set; }

        [JsonProperty("total_pages")]
        public uint TotalPages { get; set; }
    }
}
