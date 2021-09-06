using System.Threading.Tasks;

namespace Eumel.Dj.Mobile.Services
{
    public interface IPlaylistService
    {
        Task<DjPlaylist> Get();
    }
}