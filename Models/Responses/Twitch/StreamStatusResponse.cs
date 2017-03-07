using Newtonsoft.Json;

namespace Announcer.Models.Responses.Twitch
{
    public class StreamStatusResponse
    {
        [JsonProperty("stream")]
        public StreamResponse Stream { get; set; }
    }
}
