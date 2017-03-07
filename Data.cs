using Announcer.Models.Payloads;
using System.Collections.Generic;

namespace Announcer
{
    public class Data
    {
        public static Queue<MessagePayload> MessageQueue { get; set; } = new Queue<MessagePayload>();

        public static Queue<WebhookPayload> WebhookQueue { get; set; } = new Queue<WebhookPayload>();
    }
}
