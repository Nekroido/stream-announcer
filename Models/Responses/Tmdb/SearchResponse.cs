using Newtonsoft.Json;
using System.Collections.Generic;

namespace Announcer.Models.Responses.Tmdb
{
    public class SearchResponse : BasePagedResponse
    {
        [JsonProperty("results")]
        public new List<MediaResponse> Results { get; set; }
    }
}
