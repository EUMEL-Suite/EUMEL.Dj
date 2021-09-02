using System.Collections.Generic;
using System.Threading.Tasks;

namespace Eumel.Dj.Mobile.Services
{
    public interface IReadOnlyDataStore<T>
    { 
        Task<T> GetItemAsync(string id);
        Task<IEnumerable<T>> GetItemsAsync(bool forceRefresh = false);
    }
}