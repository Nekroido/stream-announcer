using System.Collections.Generic;

namespace Announcer.Models.Helpers
{
    public class MessageConfig
    {
        public string Prepend { get; set; } = "";

        public string Append { get; set; } = "";

        public List<string> Attachements { get; set; } = new List<string>();
    }
}
