using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Eumel.Dj.Mobile.Models;
using Eumel.Dj.Mobile.Services;
using Eumel.Dj.Mobile.Views;
using Xamarin.Forms;

namespace Eumel.Dj.Mobile.ViewModels
{
    public class PlaylistViewModel : BaseViewModel
    {
        protected IPlaylistService PlaylistService => DependencyService.Get<IPlaylistService>();
        protected IPlayerService PlayerService => DependencyService.Get<IPlayerService>();
        protected ISyslogService LogService => DependencyService.Get<ISyslogService>();

        public ObservableCollection<PlaylistSongItem> Items { get; }

        public Command PlayPlayerCommand { get; }

        public Command<PlaylistSongItem> ItemTapped { get; }

        public Command LoadPlaylistCommand { get; }

        public PlaylistViewModel()
        {
            Title = "Playlist";
            Items = new ObservableCollection<PlaylistSongItem>();
            LoadPlaylistCommand = new Command(async () => { await ExecuteLoadPlaylistCommand(); });
            PlayPlayerCommand = new Command(async () => await PlayerService.Play(), () => PlayerService.CanPlay);
            ItemTapped = new Command<PlaylistSongItem>(OnItemSelected);
        }

        private async Task ExecuteLoadPlaylistCommand()
        {
            IsBusy = true;

            try
            {
                Items.Clear();
                var playlist = await PlaylistService.Get();
                playlist.Songs.Where(x => x?.Id != null).ToList().ForEach(Items.Add);
            }
            catch (Exception ex)
            {
                LogService.Error(ex.Message);
            }
            finally
            {
                IsBusy = false;
            }
        }
        public void OnAppearing()
        {
            IsBusy = true;
        }
        async void OnItemSelected(PlaylistSongItem songItem)
        {
            if (songItem == null)
                return;

            await Shell.Current.GoToAsync($"{nameof(SongDetailPage)}?{nameof(SongDetailViewModel.ItemId)}={songItem.Id}");
        }
    }
}