using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Eumel.Dj.Mobile.Models;

namespace Eumel.Dj.Mobile.Services
{
    public class SongItemDataStore : IReadOnlyDataStore<SongItem>
    {
        private IDictionary<string, SongItem> _songCache;
        private readonly Lazy<SongsSource> _songSource;
        private readonly EumelDjServiceClient _service;

        private async Task<SongsSource> InitSongSource()
        {
            return await _service.GetSongsSourceAsync();
        }

        public SongItemDataStore()
        {
            // this must be a factory and injected!
            var cl = new HttpClientHandler();
            cl.ServerCertificateCustomValidationCallback = (message, certificate2, arg3, arg4) => true;
            var client = new HttpClient(cl);

            _service = new EumelDjServiceClient("https://192.168.178.37:443", client);

            _songSource = new Lazy<SongsSource>(() => InitSongSource().Result);
        }

        public Task<SongItem> GetItemAsync(string id)
        {
            return new Task<SongItem>(() => _songCache[id]);
        }

        public Task<IEnumerable<SongItem>> GetItemsAsync(bool forceRefresh = false)
        {
            if (_songCache != null) return new Task<IEnumerable<SongItem>>(() => _songCache.Values);

            var numOfSongs = _songSource.Value.NumberOfSongs;

            var songs = _service.GetSongsAsync(0, int.MaxValue).Result;
            _songCache = songs.ToDictionary(
                x => x.Id,
                x => new SongItem()
                {
                    Id = Guid.NewGuid().ToString(),
                    Artist = $"{x.Artist}",
                    Text = $"{x.Artist} - {x.Name}",
                    Location = x.Location
                });

            return new Task<IEnumerable<SongItem>>(() => _songCache.Values);
        }
    }
}