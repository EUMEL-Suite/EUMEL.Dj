using System.Net.Http;
using System.Threading.Tasks;

namespace Eumel.Dj.Mobile.Services
{
    public interface ISongStore<T> :IReadOnlySongStore<T>
    {
        Task<bool> AddItemAsync(T item);
        Task<bool> UpdateItemAsync(T item);
        Task<bool> DeleteItemAsync(string id);
    }

    public class PlaylistItemStore : IPlaylistItemStore
    {
        private readonly EumelDjServiceClient _service;

        public PlaylistItemStore()
        {
            // this must be a factory and injected!
            var cl = new HttpClientHandler();
            cl.ServerCertificateCustomValidationCallback = (message, certificate2, arg3, arg4) => true;
            var client = new HttpClient(cl);

            _service = new EumelDjServiceClient("https://192.168.178.37:443", client);
        }
        public async Task<DjPlaylist> Get()
        {
            return await _service.GetPlaylistAsync();
        }
    }
}
