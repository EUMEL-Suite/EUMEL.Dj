using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Eumel.Dj.Mobile.Models;
using Xamarin.Forms;

namespace Eumel.Dj.Mobile.ViewModels
{
    public class SongsViewModel : BaseViewModel
    {
        private SongItem _selectedSongItem;

        public SongsViewModel()
        {
            Title = "Browse";
            Items = new ObservableCollection<SongItem>();
            LoadItemsCommand = new Command(async () => { await ExecuteLoadItemsCommand(); });

            ItemTapped = new Command<SongItem>(OnItemSelected);
            VoteUpDownCommand = new Command<SongItem>(OnItemUpDownVote);
        }

        public ObservableCollection<SongItem> Items { get; }
        public Command LoadItemsCommand { get; }
        public Command<SongItem> ItemTapped { get; }

        public Command<SongItem> VoteUpDownCommand { get; }

        public SongItem SelectedSongItem
        {
            get => _selectedSongItem;
            set
            {
                SetProperty(ref _selectedSongItem, value);
                OnItemSelected(value);
            }
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

        private void OnItemUpDownVote(SongItem song)
        {
            TryOrRedirectToLoginAsync(async () =>
            {
                var hasMyVote = await SongService.Vote(song.Id);
                song.HasMyVote = hasMyVote;
            });
        }

        private async void OnItemSelected(SongItem songItem)
        {
            if (songItem == null)
                return;

            // we don't show a detail page any longer
        }
    }
}