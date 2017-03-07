using System.Collections.Generic;

namespace Announcer.Models.Payloads
{
    public class WebhookPayload
    {
        public Dashboard Dashboard { get; set; }

        public List<ChannelInfo> ChannelInfos { get; set; } = new List<ChannelInfo>();
    }
}
