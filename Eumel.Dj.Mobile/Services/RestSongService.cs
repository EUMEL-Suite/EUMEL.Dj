using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Eumel.Dj.Mobile.Models;
using Xamarin.Forms;

namespace Eumel.Dj.Mobile.Services
{
    public class RestSongService : ISongService
    {
        private readonly ISyslogService _log;
        private readonly EumelDjServiceClient _service;
        private readonly ISettingsService _settings;
        private IDictionary<string, SongItem> _songCache;

        public RestSongService()
        {
            _log = DependencyService.Get<ISyslogService>();
            _service = DependencyService.Get<IEumelRestServiceFactory>().Build();
            _settings = DependencyService.Get<ISettingsService>();
            _songCache = new ConcurrentDictionary<string, SongItem>();
        }

        public async Task<SongListItem> GetSongsAsync(bool forceRefresh = false)
        {
            _log.Information($"User {_settings.Username} a list of songs");
            var source = await _service.GetSongsSourceAsync();

            var myVotes = await _service.GetMyVotesAsync();

            var songs = await _service.GetSongsAsync(0, int.MaxValue);

            _songCache = songs.ToDictionary(
                x => x.Id,
                x => x.ToSongItem(myVotes.Any(y => y.Id == x.Id)));

            return new SongListItem()
            {
                Name = source.SourceName,
                NumberOfSongs = source.NumberOfSongs,
                Songs = _songCache.Values
            };
        }

        public async Task<bool> Vote(string id)
        {
            _ = _songCache.TryGetValue(id, out var song);

            _log.Debug($"Call get my votes service endpoint for {_settings.Username}");
            var myVotes = await _service.GetMyVotesAsync();
            var hasAlreadyVoted = myVotes.Any(y => y.Id == id);

            if (hasAlreadyVoted)
            {
                _log.Information($"User {_settings.Username} has already voted. Remove vote for {id} [{song?.Title}]");
                _log.Debug($"Call down vote service endpoint");
                await _service.DownVoteAsync(new Song() { Id = id });
                return false;
            }
            else
            {
                _log.Information($"User {_settings.Username} has voted for {id} [{song?.Title}]");
                _log.Debug($"Call up vote service endpoint");
                await _service.UpVoteAsync(new Song() { Id = id });
                return true;
            }
        }

        public async Task<SongItem> GetItemAsync(string id)
        {
            return _songCache[id];
        }
    }
}