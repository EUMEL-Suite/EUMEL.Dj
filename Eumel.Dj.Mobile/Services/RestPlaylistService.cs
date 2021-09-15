using System.Linq;
using System.Threading.Tasks;
using Eumel.Dj.Mobile.Models;
using Xamarin.Forms;

namespace Eumel.Dj.Mobile.Services
{
    public class RestPlaylistService : IPlaylistService
    {
        private ISyslogService Log => DependencyService.Get<ISyslogService>();
        private EumelDjServiceClient Service => DependencyService.Get<IEumelRestServiceFactory>().Build();
        private ISettingsService Settings =>DependencyService.Get<ISettingsService>();

        public async Task<PlaylistItem> Get()
        {
            Log.Debug("Getting playlist");
            var serverPlaylist = await Service.GetPlaylistAsync();

            var songs = serverPlaylist.PastSongs.Select(x => x.ToPlaylistSongItem(SongType.Past, Settings))
                .Append(serverPlaylist.CurrentSong.ToPlaylistSongItem(SongType.Current, Settings))
                .Concat(serverPlaylist.UpcomingSongs.Select(x => x.ToPlaylistSongItem(SongType.Upcomming, Settings)));

            return new PlaylistItem()
            {
                Songs = songs
            };
        }

        public async Task ClearMyVotes()
        {
            await Service.ClearMyVotesAsync();
        }
    }
}