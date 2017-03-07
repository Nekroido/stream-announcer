using Newtonsoft.Json;
using System.Collections.Generic;

namespace Announcer.Models.Responses.GiantBomb
{
    public class GameSearchResponse : BasePagedResponse
    {
        [JsonProperty("results")]
        public new List<GameResponse> Results { get; set; }
    }
}
