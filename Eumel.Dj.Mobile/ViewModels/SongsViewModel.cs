using Eumel.Dj.Mobile.Models;
using Eumel.Dj.Mobile.Views;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Eumel.Dj.Mobile.Services;
using Xamarin.Forms;

namespace Eumel.Dj.Mobile.ViewModels
{
    public class SongsViewModel : BaseViewModel
    {
        private SongItem _selectedSongItem;

        public ObservableCollection<SongItem> Items { get; }
        public Command LoadItemsCommand { get; }
        public Command<SongItem> ItemTapped { get; }

        public Command<SongItem> VoteUpDownCommand { get; }

        public ISongService SongService => DependencyService.Get<ISongService>();
        public SongsViewModel()
        {
            Title = "Browse";
            Items = new ObservableCollection<SongItem>();
            LoadItemsCommand = new Command(async () => { await ExecuteLoadItemsCommand(); });

            ItemTapped = new Command<SongItem>(OnItemSelected);
            VoteUpDownCommand = new Command<SongItem>(OnItemUpDownVote);
        }

        private async Task ExecuteLoadItemsCommand()
        {
            IsBusy = true;

            try
            {
                Items.Clear();
                var source = await SongService.GetSongsAsync(true);
                source.Songs.Where(x => x?.Id != null).ToList().ForEach(Items.Add);
                Title = $"Browse Playlist '{source.Name}' [{source.NumberOfSongs} Songs]";
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
            SelectedSongItem = null;
        }

        public SongItem SelectedSongItem
        {
            get => _selectedSongItem;
            set
            {
                SetProperty(ref _selectedSongItem, value);
                OnItemSelected(value);
            }
        }

        private async void OnItemUpDownVote(SongItem song)
        {
            await SongService.Vote(song.Id);
        }

        async void OnItemSelected(SongItem songItem)
        {
            if (songItem == null)
                return;

            // This will push the ItemDetailPage onto the navigation stack
            await Shell.Current.GoToAsync($"{nameof(SongDetailPage)}?{nameof(SongDetailViewModel.ItemId)}={songItem.Id}");
        }
    }
}