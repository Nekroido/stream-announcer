using Announcer.Classes.Services;
using Announcer.Interfaces;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Announcer.Models.Responses.Tmdb
{
    public class MediaResponse : IMediaItem
    {
        [JsonProperty("id")]
        public uint Id { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("poster_path")]
        private string _posterPath { get; set; }

        public string Poster
        {
            get
            {
                return string.Format(TmdbService.IMAGE_URL, _posterPath);
            }
        }

        public List<uint> PlatformIds => new List<uint>();
    }
}
