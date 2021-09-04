using System.Collections.Generic;
using System.Threading.Tasks;
using Eumel.Dj.Mobile.Models;

namespace Eumel.Dj.Mobile.Services
{
    public interface IReadOnlySongStore<T>
    { 
        Task<T> GetItemAsync(string id);
        Task<IEnumerable<T>> GetItemsAsync(bool forceRefresh = false);
        Task<SongSourceItem> GetSourceAsync(bool forceRefresh = false);
    }
}