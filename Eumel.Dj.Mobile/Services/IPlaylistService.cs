using System.Threading.Tasks;
using Eumel.Dj.Mobile.Models;

namespace Eumel.Dj.Mobile.Services
{
    public interface IPlaylistService
    {
        Task<PlaylistItem> Get();
    }
}