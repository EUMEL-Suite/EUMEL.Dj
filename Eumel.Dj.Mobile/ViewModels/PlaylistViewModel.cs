using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using Eumel.Dj.Mobile.Models;
using Eumel.Dj.Mobile.Services;
using Xamarin.Forms;

namespace Eumel.Dj.Mobile.ViewModels
{
    public class PlaylistViewModel : BaseViewModel
    {
        protected IPlaylistService PlaylistService => DependencyService.Get<IPlaylistService>();

        public ObservableCollection<VotedSong> Items { get; }

        public Command LoadPlaylistCommand { get; }

        public PlaylistViewModel()
        {
            Title = "Playlist";
            Items = new ObservableCollection<VotedSong>();
            LoadPlaylistCommand = new Command(async () => { await ExecuteLoadPlaylistCommand(); });
        }

        private async Task ExecuteLoadPlaylistCommand()
        {
            IsBusy = true;

            try
            {
                Items.Clear();
                var playlist = await PlaylistService.Get();
                var items = playlist.UpcomingSongs;
                foreach (var item in items)
                {
                    Items.Add(item);
                }
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