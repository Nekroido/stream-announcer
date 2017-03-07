using System.Collections.Generic;

namespace Announcer.Interfaces
{
    public interface IMediaItem
    {
        uint Id { get; set; }

        string Title { get; set; }

        string Poster { get; }

        List<uint> PlatformIds { get; }
    }
}
