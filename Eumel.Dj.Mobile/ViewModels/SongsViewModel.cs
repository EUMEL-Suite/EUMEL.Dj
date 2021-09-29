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
        private string _searchText;

        public SongsViewModel()
        {
            Title = "Browse/Find songs";
            Items = new ObservableCollection<SongItem>();
            LoadItemsCommand = new Command(async () => { await ExecuteLoadItemsCommand(); });

            VoteUpDownCommand = new Command<SongItem>(OnItemUpDownVote);
            PerformSearchCommand = new Command<string>(async x =>
            {
                SyslogService.Information($"Executing a search for '{x}'");
                OnPropertyChanged(nameof(Items));
                await DoExecuteSearch(x);
                return;
            });
        }

        private async Task DoExecuteSearch(string searchText)
        {
            IsBusy = true;

            try
            {
                var source = string.IsNullOrWhiteSpace(searchText)
                    ? await SongService.GetSongsAsync()
                    : await SongService.SearchSongsAsync(searchText);

                lock (Items) // the refresh thing is executing it twice and it runs into a race condition
                // probably we can store the "result is for search xy" in a field and abort if the result is already shown for a specific search
                {
                    Items.Clear();
                    source.Songs.Where(x => x?.Id != null).ToList().ForEach(Items.Add);
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

        public ObservableCollection<SongItem> Items { get; }
        public Command LoadItemsCommand { get; }
        public Command<SongItem> VoteUpDownCommand { get; }
        public Command<string> PerformSearchCommand { get; }

        public string SearchText
        {
            get => _searchText; 
            set => SetProperty(ref _searchText, value, onChanged: async() =>
            {
                if (string.IsNullOrWhiteSpace(_searchText)) await DoExecuteSearch(_searchText);
            });
        }

        public SongItem SelectedSongItem
        {
            get => _selectedSongItem;
            set => SetProperty(ref _selectedSongItem, value);
        }

        private async Task ExecuteLoadItemsCommand()
        {
            SyslogService.Information($"Executing a song list ('{SearchText}')");
            await DoExecuteSearch(SearchText);
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
            }, "Vote song");
        }
    }
}