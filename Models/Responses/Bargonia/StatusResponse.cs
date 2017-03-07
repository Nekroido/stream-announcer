using Newtonsoft.Json;

namespace Announcer.Models.Responses.Bargonia
{
    public class StatusResponse
    {
        [JsonProperty("bargonia")]
        public DetailsResponse Bargonia { get; set; }
    }
}
