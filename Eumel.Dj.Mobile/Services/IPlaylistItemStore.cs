using System.Threading.Tasks;

namespace Eumel.Dj.Mobile.Services
{
    public interface IPlaylistItemStore
    {
        Task<DjPlaylist> Get();
    }
}