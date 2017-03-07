using Newtonsoft.Json;
using System.Collections.Generic;

namespace Announcer.Models.Helpers
{
    public class ChannelConfig
    {
        [JsonProperty("type")]
        public ChannelType Type { get; set; }

        [JsonProperty("channel_id")]
        public uint ChannelId { get; set; }

        [JsonProperty("channel")]
        public string Channel { get; set; }

        [JsonProperty("enabled")]
        public bool IsEnabled { get; set; } = false;

        [JsonProperty("liveWaitDelay")]
        public uint LiveWaitDelay { get; set; } = 60000;

        [JsonProperty("announce")]
        public List<AnnounceConfig> Announce { get; set; } = new List<AnnounceConfig>();

        [JsonProperty("stream")]
        public List<StreamConfig> Stream { get; set; } = new List<StreamConfig>();
    }
}
