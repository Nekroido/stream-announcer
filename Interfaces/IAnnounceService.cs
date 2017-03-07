using Announcer.Models.Payloads;
using System.Threading.Tasks;

namespace Announcer.Interfaces
{
    public interface IAnnounceService
    {
        Task<uint> Announce(MessagePayload payload);
    }
}
