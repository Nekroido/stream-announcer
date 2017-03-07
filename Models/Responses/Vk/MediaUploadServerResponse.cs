using Newtonsoft.Json;

namespace Announcer.Models.Responses.Vk
{
    public class MediaUploadServerResponse
    {
        [JsonProperty("upload_url")]
        public string UploadUrl { get; set; }

        [JsonProperty("album_id")]
        public int AlbumId { get; set; }

        [JsonProperty("user_id")]
        public int UserId { get; set; }
    }
}
