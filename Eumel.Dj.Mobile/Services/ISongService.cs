using System.Threading.Tasks;
using Eumel.Dj.Mobile.Models;

namespace Eumel.Dj.Mobile.Services
{
    public interface ISongService
    {
        Task<SongListItem> GetSongsAsync(bool forceRefresh = false);
        Task<bool> Vote(string id);
        Task<SongItem> GetItemAsync(string id);
        Task<SongListItem> SearchSongsAsync(string searchText);
    }
}