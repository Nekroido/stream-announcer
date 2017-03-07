using Announcer.Interfaces;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace Announcer.Models.Responses.GiantBomb
{
    public class GameResponse : IMediaItem
    {
        [JsonProperty("id")]
        public uint Id { get; set; }

        [JsonProperty("name")]
        public string Title { get; set; }

        [JsonProperty("image")]
        public ImageResponse Image { get; set; }

        [JsonProperty("platforms")]
        public List<PlatformResponse> Platforms { get; set; }

        public string Poster => Image?.SuperUrl ?? "";

        public List<uint> PlatformIds => Platforms?.Count > 0 ? Platforms.Select(x => x.Id).ToList() : new List<uint>();
    }
}
