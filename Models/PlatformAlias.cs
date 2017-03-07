using Announcer.Db;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Announcer.Models
{
    public class PlatformAlias
    {
        public uint Id { get; set; }

        public string Alias { get; set; }

        [ForeignKey("PlatformForeignKey")]
        public Platform Platform { get; set; }

        public static async Task<Platform> FindPlatform(string dashboard)
        {
            using (var context = new Context())
            {
                var aliases = await context.PlatformAlias.Include(x => x.Platform).OrderByDescending(x => x.Alias).ToListAsync();

                foreach (var alias in aliases)
                {
                    if (Regex.IsMatch(dashboard, @"\b(" + alias.Alias + @")\b", RegexOptions.IgnoreCase))
                    {
                        return alias.Platform;
                    }
                }
            }

            return null;
        }
    }
}
