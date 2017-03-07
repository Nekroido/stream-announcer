using Newtonsoft.Json;

namespace Announcer.Models.Responses.Vk
{
    public class Photo
    {
        [JsonProperty("id")]
        public uint Id { get; set; }

        [JsonProperty("album_id")]
        public int AlbumId { get; set; }

        [JsonProperty("owner_id")]
        public int OwnerId { get; set; }

        [JsonProperty("photo_75")]
        public string Photo75 { get; set; }

        [JsonProperty("photo_130")]
        public string Photo130 { get; set; }

        [JsonProperty("photo_604")]
        public string Photo604 { get; set; }

        [JsonProperty("photo_807")]
        public string Photo807 { get; set; }

        [JsonProperty("photo_1280")]
        public string Photo1280 { get; set; }

        [JsonProperty("photo_2560")]
        public string Photo2560 { get; set; }
    }
}
