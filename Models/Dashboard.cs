using Announcer.Db;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Announcer.Models
{
    public class Dashboard
    {
        [JsonIgnore]
        public uint Id { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; } = "";

        [JsonProperty("media")]
        public string Media { get; set; } = "";

        [JsonProperty("channel_id")]
        public uint ChannelId { get; set; } = 0;

        [JsonProperty("channel")]
        public string Channel { get; set; } = "";

        [JsonProperty("broadcast_start")]
        public DateTime BroadcastStart { get; set; } = DateTime.Now;

        [JsonProperty("broadcast_end")]
        public DateTime? BroadcastEnd { get; set; } = null;

        [JsonProperty("viewers")]
        public uint Viewers { get; set; } = 0;

        [JsonProperty("max_viewers")]
        public uint MaxViewers { get; set; } = 0;

        [JsonProperty("vk_post_id")]
        public uint? VkPostId { get; set; } = null;

        [JsonProperty("is_live")]
        public bool IsLive { get; set; } = false;

        public async Task Save()
        {
            using (var context = new Context())
            {
                if (Id.Equals(default(uint)))
                {
                    context.Dashboards.Add(this);
                }
                else
                {
                    context.Dashboards.Attach(this);
                    context.Entry(this).State = EntityState.Modified;
                }
                await context.SaveChangesAsync();
            }
        }

        public static Dashboard GetPrevious(string channel)
        {
            using (var context = new Context())
            {
                return context.Dashboards.Where(x => x.Channel == channel).OrderByDescending(x => x.Id).FirstOrDefault();
            }
        }

        public bool IsPrevious(Dashboard previousDashboard)
        {
            if (previousDashboard == null)
                return false;
            else if (this.Title == previousDashboard.Title)
                return (this.Media == previousDashboard.Media || Config.Load().IgnoredGames.IndexOf(this.Media) > -1);
            else return false;
        }
    }
}
