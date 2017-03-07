using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Announcer.Models
{
    public class Platform
    {
        [Key]
        public string Abbreviation { get; set; }

        public string Name { get; set; }

        public uint GiantBombId { get; set; }

        public List<PlatformAlias> Aliases { get; set; }

        public List<GamePlatform> GamePlatforms { get; set; }
    }
}
