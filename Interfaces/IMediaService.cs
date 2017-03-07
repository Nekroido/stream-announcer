using Announcer.Models;
using System.Threading.Tasks;

namespace Announcer.Interfaces
{
    public interface IMediaService
    {
        Task<Media> findMedia(string title, string dashboard);

        Task<Media> findMedia(string dashboard);
    }
}
