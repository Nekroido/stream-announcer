using Newtonsoft.Json;

namespace Announcer.Models.Responses.GiantBomb
{
    public class ImageResponse
    {
        [JsonProperty("icon_url")]
        public string IconUrl { get; set; }

        [JsonProperty("medium_url")]
        public string MediumUrl { get; set; }

        [JsonProperty("screen_url")]
        public string ScreenUrl { get; set; }

        [JsonProperty("small_url")]
        public string SmallUrl { get; set; }

        [JsonProperty("super_url")]
        public string SuperUrl { get; set; }

        [JsonProperty("thumb_url")]
        public string ThumbUrl { get; set; }

        [JsonProperty("tiny_url")]
        public string TinyUrl { get; set; }
    }
}
