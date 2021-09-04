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
        private readonly Lazy<Task<SongsSource>> _songSource;
        private readonly EumelDjServiceClient _service;

        private async Task<SongsSource> InitSongSource()
        {
            return await _service.GetSongsSourceAsync();
        }

        public SongItemSongStore()
        {
            // this must be a factory and injected!
            var cl = new HttpClientHandler();
            cl.ServerCertificateCustomValidationCallback = (message, certificate2, arg3, arg4) => true;
            var client = new HttpClient(cl);

            _service = new EumelDjServiceClient("https://192.168.178.37:443", client);

            _songSource = new Lazy<Task<SongsSource>>(InitSongSource);
        }

        public Task<SongItem> GetItemAsync(string id)
        {
            return new Task<SongItem>(() => _songCache[id]);
        }

        public async Task<IEnumerable<SongItem>> GetItemsAsync(bool forceRefresh = false)
        {
            if (_songCache != null) return _songCache.Values;

            var songSource = await _songSource.Value;
            var numOfSongs = songSource.NumberOfSongs;

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
            var source = await _songSource.Value;
            return new SongSourceItem() { Name = source.SourceName, NumberOfSongs = source.NumberOfSongs };
        }
    }
}