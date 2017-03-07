namespace Announcer.Models
{
    public class GamePlatform
    {
        public uint GameId { get; set; }
        public Game Game { get; set; }

        public string PlatformAbbreviation { get; set; }
        public Platform Platform { get; set; }
    }
}
