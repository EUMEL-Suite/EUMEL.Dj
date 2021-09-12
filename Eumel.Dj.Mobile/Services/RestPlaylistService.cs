using System.Linq;
using System.Threading.Tasks;
using Eumel.Dj.Mobile.Models;
using Xamarin.Forms;

namespace Eumel.Dj.Mobile.Services
{
    public class RestPlaylistService : IPlaylistService
    {

        private readonly ISyslogService _log;
        private readonly EumelDjServiceClient _service;
        private readonly ISettingsService _settings;

        public RestPlaylistService()
        {
            _log = DependencyService.Get<ISyslogService>();
            _service = DependencyService.Get<IEumelRestServiceFactory>().Build();
            _settings = DependencyService.Get<ISettingsService>();
        }

        public async Task<PlaylistItem> Get()
        {
            _log.Debug("Getting playlist");
            var serverPlaylist = await _service.GetPlaylistAsync();

            var songs = serverPlaylist.PastSongs.Select(x => x.ToPlaylistSongItem(SongType.Past, _settings))
                .Append(serverPlaylist.CurrentSong.ToPlaylistSongItem(SongType.Current, _settings))
                .Concat(serverPlaylist.UpcomingSongs.Select(x => x.ToPlaylistSongItem(SongType.Upcomming, _settings)));

            return new PlaylistItem()
            {
                Songs = songs
            };
        }

        public async Task ClearMyVotes()
        {
            await _service.ClearMyVotesAsync();
        }
    }
}