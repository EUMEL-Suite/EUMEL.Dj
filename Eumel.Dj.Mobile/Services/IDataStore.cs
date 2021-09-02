using System.Threading.Tasks;

namespace Eumel.Dj.Mobile.Services
{
    public interface IDataStore<T> :IReadOnlyDataStore<T>
    {
        Task<bool> AddItemAsync(T item);
        Task<bool> UpdateItemAsync(T item);
        Task<bool> DeleteItemAsync(string id);
    }
}
