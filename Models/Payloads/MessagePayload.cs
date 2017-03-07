using Announcer.Models.Helpers;

namespace Announcer.Models.Payloads
{
    public class MessagePayload
    {
        public string Message { get; set; }

        public string PosterPath { get; set; }

        public ChannelConfig ChannelConfig { get; set; }

        public Dashboard Dashboard { get; set; }

        public Dashboard PreviousDashboard { get; set; }

        public AnnounceType? AnnounceFor { get; set; }
    }
}
