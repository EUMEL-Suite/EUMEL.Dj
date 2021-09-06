using Eumel.Dj.Mobile.Models;
using Eumel.Dj.Mobile.Views;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Eumel.Dj.Mobile.Services;
using Xamarin.Forms;

namespace Eumel.Dj.Mobile.ViewModels
{
    public class ItemsViewModel : BaseViewModel
    {
        private SongItem _selectedSongItem;

        public ObservableCollection<SongItem> Items { get; }
        public SongSourceItem Source { get; set; }
        public Command LoadItemsCommand { get; }
        public Command AddItemCommand { get; }
        public Command<SongItem> ItemTapped { get; }

        public IReadOnlySongStore<SongItem> SongStore => DependencyService.Get<IReadOnlySongStore<SongItem>>();
        public ItemsViewModel()
        {
            Title = "Browse";
            Items = new ObservableCollection<SongItem>();
            Source = SongStore.GetSourceAsync(true).Result;
            Title = $"Browse Playlist '{Source.Name}' [{Source.NumberOfSongs} Songs]";
            LoadItemsCommand = new Command(async () => { await ExecuteLoadItemsCommand(); });

            ItemTapped = new Command<SongItem>(OnItemSelected);

            AddItemCommand = new Command(OnAddItem);
        }

        async Task ExecuteLoadItemsCommand()
        {
            IsBusy = true;

            try
            {
                Items.Clear();
                var items = await SongStore.GetItemsAsync(true);
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

        private async void OnAddItem(object obj)
        {
            await Shell.Current.GoToAsync(nameof(NewItemPage));
        }

        async void OnItemSelected(SongItem songItem)
        {
            if (songItem == null)
                return;

            // This will push the ItemDetailPage onto the navigation stack
            await Shell.Current.GoToAsync($"{nameof(ItemDetailPage)}?{nameof(ItemDetailViewModel.ItemId)}={songItem.Id}");
        }
    }
}