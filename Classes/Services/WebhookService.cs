using Announcer.Classes.Helpers;
using Announcer.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Announcer.Classes.Services
{
    public class WebhookService : ApiBase
    {
        public async Task Publish(Dashboard dasboard, List<ChannelInfo> channelInfos, string url)
        {
            var payload = JsonConvert.DeserializeObject<Dictionary<string, object>>(JsonConvert.SerializeObject(dasboard));
            payload.Add("timestamp", UnixTimeHelper.GetCurrentUnixTimestampSeconds());

            var streams = new List<Dictionary<string, string>>();
            foreach (var info in channelInfos)
            {
                streams.Add(JsonConvert.DeserializeObject<Dictionary<string, string>>(JsonConvert.SerializeObject(info)));
            }

            payload.Add("streams", streams);

            var content = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");

            await JsonRequest(url, content);
        }
    }
}
