using Announcer.Models.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Announcer
{
    public class Config
    {
        [JsonProperty("delay")]
        public uint Delay { get; set; }

        [JsonProperty("cacheUpdateDelay")]
        public uint CacheUpdateDelay { get; set; }

        [JsonProperty("debug")]
        public bool Debug { get; set; } = false;

        [JsonProperty("enabled")]
        public bool IsEnabled { get; set; } = false;

        [JsonProperty("errorPercent")]
        public uint ErrorPercentage { get; set; } = 70;

        [JsonProperty("removeStringsFromTitle")]
        public List<string> RemoveStringsFromTitle { get; set; }

        [JsonProperty("ignoredGames")]
        public List<string> IgnoredGames { get; set; }

        [JsonProperty("services")]
        public List<ServiceConfig> Services { get; set; }

        [JsonProperty("statistics_webhooks")]
        public List<string> StatisticsWebhooks { get; set; }

        [JsonProperty("channels")]
        public List<ChannelConfig> Channels { get; set; }

        public static Config Load()
        {
            try
            {
                var path = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "config.json");
                var data = JsonConvert.DeserializeObject<Config>(File.ReadAllText(path));

#if DEBUG
                try
                {
                    var discord = data.Services.FirstOrDefault(x => x.Name == "discord");
                    discord.Token = "-your debug discord webhook token-";
                    discord.Id = "-your debug discord webhook id-";

                    var vk = data.Services.FirstOrDefault(x => x.Name == "vk");
                    vk.Id = "-your debug public page id-";

                    data.StatisticsWebhooks = new List<string>();
                }
                catch { }
#endif

                return data;
            }
            catch
            {
                return null;
            }
        }

        public static ServiceConfig GetServiceConfig(string serviceName)
        {
            return Load().Services.FirstOrDefault(x => x.Name == serviceName);
        }
    }
}
