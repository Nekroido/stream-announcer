using Newtonsoft.Json;
using System.Collections.Generic;

namespace Announcer.Models.Responses.GiantBomb
{
    public class BasePagedResponse : BaseResponse
    {
        [JsonProperty("limit")]
        public uint Limit { get; set; }

        [JsonProperty("offset")]
        public uint Offset { get; set; }

        [JsonProperty("number_of_page_results")]
        public uint TotalResultsOnPage { get; set; }

        [JsonProperty("number_of_total_results")]
        public uint TotalResults { get; set; }

        [JsonProperty("results")]
        public List<object> Results { get; set; }
    }
}
