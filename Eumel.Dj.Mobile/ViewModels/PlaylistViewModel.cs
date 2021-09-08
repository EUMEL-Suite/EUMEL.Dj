using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Eumel.Dj.Mobile.Models;
using Eumel.Dj.Mobile.Services;
using Xamarin.Forms;

namespace Eumel.Dj.Mobile.ViewModels
{
    public class PlaylistViewModel : BaseViewModel
    {
        private string _likeBackgroundImage;

        protected IPlaylistService PlaylistService => DependencyService.Get<IPlaylistService>();

        public ObservableCollection<PlaylistSongItem> Items { get; }

        public Command LoadPlaylistCommand { get; }

        public string LikeBackgroundImage
        {
            get => _likeBackgroundImage;
            set => SetProperty(ref _likeBackgroundImage, value);
        }


        public PlaylistViewModel()
        {
            Title = "Playlist";
            Items = new ObservableCollection<PlaylistSongItem>();
            LoadPlaylistCommand = new Command(async () => { await ExecuteLoadPlaylistCommand(); });
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
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }
        public void OnAppearing()
        {
            IsBusy = true;
            //SelectedVotedSong = null;
        }

    }
}