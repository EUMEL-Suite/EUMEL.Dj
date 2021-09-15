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
        private ISyslogService Log => DependencyService.Get<ISyslogService>();
        private EumelDjServiceClient Service => DependencyService.Get<IEumelRestServiceFactory>().Build();
        private ISettingsService Settings => DependencyService.Get<ISettingsService>();
        private IDictionary<string, SongItem> _songCache = new ConcurrentDictionary<string, SongItem>();

        public async Task<SongListItem> GetSongsAsync(bool forceRefresh = false)
        {
            Log.Information($"User {Settings.Username} a list of songs");
            var source = await Service.GetSongsSourceAsync();

            var myVotes = await Service.GetMyVotesAsync();

            var songs = await Service.GetSongsAsync(0, int.MaxValue);

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

            Log.Debug($"Call get my votes service endpoint for {Settings.Username}");
            var myVotes = await Service.GetMyVotesAsync();
            var hasAlreadyVoted = myVotes.Any(y => y.Id == id);

            if (hasAlreadyVoted)
            {
                Log.Information($"User {Settings.Username} has already voted. Remove vote for {id} [{song?.Title}]");
                Log.Debug($"Call down vote service endpoint");
                await Service.DownVoteAsync(new Song() { Id = id });
                return false;
            }
            else
            {
                Log.Information($"User {Settings.Username} has voted for {id} [{song?.Title}]");
                Log.Debug($"Call up vote service endpoint");
                await Service.UpVoteAsync(new Song() { Id = id });
                return true;
            }
        }

        public async Task<SongItem> GetItemAsync(string id)
        {
            return _songCache[id];
        }
    }
}