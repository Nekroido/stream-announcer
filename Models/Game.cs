using System.Collections.Generic;

namespace Announcer.Models
{
    public class Game
    {
        public uint Id { get; set; }

        public string Title { get; set; }

        public uint GiantBombId { get; set; }

        public List<GamePlatform> GamePlatforms { get; set; }
    }
}
