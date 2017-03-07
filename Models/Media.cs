using Announcer.Classes.Helpers;
using Announcer.Classes.Services;
using Announcer.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Announcer.Models
{
    public class Media
    {
        public uint Id { get; set; }

        public string Title { get; set; }

        public string Poster { get; set; }

        public List<uint> Platforms { get; set; }

        public static async Task<Media> Find(ChannelType type, Dashboard dashboard)
        {
            List<IMediaItem> items;

            switch (type)
            {
                case ChannelType.movies:
                    items = (await TmdbService.Instance.FindMovie(dashboard.Title)).ToList<IMediaItem>();
                    break;
                default:
                    var platform = await PlatformAlias.FindPlatform(dashboard.Title);
                    items = (await GiantBombService.Instance.FindGame(dashboard.Media, platform?.GiantBombId)).ToList<IMediaItem>();
                    break;
            }

            var bestItem = bestMatch(items, dashboard.Media);

            return bestItem != null ? new Media
            {
                Id = bestItem.Id,
                Title = bestItem.Title,
                Poster = bestItem.Poster,
                Platforms = bestItem.PlatformIds
            } : null;
        }

        private static IMediaItem bestMatch(List<IMediaItem> items, string title)
        {
            IMediaItem bestItem = null;
            double bestPercent = 0;

            foreach (var item in items)
            {
                var percent = StringHelper.CalculateSimilarity(item.Title, title);

                if (percent > bestPercent)
                {
                    bestPercent = percent;
                    bestItem = item;
                }
            }

            return bestItem;
        }
    }
}
