using Announcer.Classes.Services;
using Announcer.Interfaces;
using Announcer.Models.Helpers;

namespace Announcer.Classes
{
    public class ServiceFactory
    {
        public static IAnnounceService GetAnnounceService(AnnounceConfig config)
        {
            switch (config.Type)
            {
                case AnnounceType.vk:
                    return new VkService(config);
                case AnnounceType.discord:
                    return new DiscordService(config);
                default:
                    return null;
            }
        }

        public static IStreamInfoService GetStreamInfoService(StreamConfig config)
        {
            switch (config.Type)
            {
                case StreamType.twitch:
                    return new TwitchService(config);
                case StreamType.hitbox:
                    return new HitboxService(config);
                case StreamType.youtube:
                    return new YoutubeService(config);
                case StreamType.beam:
                    return new BeamService(config);
                case StreamType.cybergame:
                    return new CyberGameService(config);
                case StreamType.goodgame:
                    return new GoodGameService(config);
                case StreamType.bargonia:
                    return new BargoniaService(config);
                default:
                    return null;
            }
        }
    }
}
