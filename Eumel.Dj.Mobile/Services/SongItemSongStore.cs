using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Eumel.Dj.Mobile.Models;

namespace Eumel.Dj.Mobile.Services
{
    public class SongItemSongStore : IReadOnlySongStore<SongItem>
    {
        private IDictionary<string, SongItem> _songCache;
        private readonly EumelDjServiceClient _service;
        private SongsSource Source;

        private async Task InitSongSource()
        {
            if (Source != null) return;
            Source = await _service.GetSongsSourceAsync();
        }

        public SongItemSongStore()
        {
            // this must be a factory and injected!
            var cl = new HttpClientHandler();
            cl.ServerCertificateCustomValidationCallback = (message, certificate2, arg3, arg4) => true;
            var client = new HttpClient(cl);

            _service = new EumelDjServiceClient("https://192.168.178.37:443", client);
        }

        public Task<SongItem> GetItemAsync(string id)
        {
            return new Task<SongItem>(() => _songCache[id]);
        }

        public async Task<IEnumerable<SongItem>> GetItemsAsync(bool forceRefresh = false)
        {
            if (_songCache != null) return _songCache.Values;
            await InitSongSource();

            var numOfSongs = Source.NumberOfSongs;

            var songs = await _service.GetSongsAsync(0, int.MaxValue);

            _songCache = songs.ToDictionary(
                x => x.Id,
                x => new SongItem()
                {
                    Id = x.Id,
                    Artist = $"{x.Artist}",
                    Text = $"{x.Artist} - {x.Name}",
                });

            return _songCache.Values;
        }

        public async Task<SongSourceItem> GetSourceAsync(bool forceRefresh = false)
        {
            Source = _service.GetSongsSourceAsync().Result;
            return new SongSourceItem() { Name = Source.SourceName, NumberOfSongs = Source.NumberOfSongs };
        }
    }
}