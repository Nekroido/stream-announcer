using Announcer.Models;
using System.Threading.Tasks;

namespace Announcer.Interfaces
{
    public interface IStreamInfoService
    {
        Task<ChannelInfo> GetStreamInfoAsync(string channel);
    }
}
