using System;
using System.Diagnostics;
using Eumel.Dj.Mobile.Models;
using Eumel.Dj.Mobile.Services;
using Xamarin.Forms;

namespace Eumel.Dj.Mobile.ViewModels
{
    [QueryProperty(nameof(ItemId), nameof(ItemId))]
    public class ItemDetailViewModel : BaseViewModel
    {
        public IReadOnlySongStore<SongItem> SongStore => DependencyService.Get<IReadOnlySongStore<SongItem>>();

        private string itemId;
        private string text;
        private string description;
        public string Id { get; set; }

        public string Text
        {
            get => text;
            set => SetProperty(ref text, value);
        }

        public string Description
        {
            get => description;
            set => SetProperty(ref description, value);
        }

        public string ItemId
        {
            get
            {
                return itemId;
            }
            set
            {
                itemId = value;
                LoadItemId(value);
            }
        }

        public async void LoadItemId(string itemId)
        {
            try
            {
                var item = await SongStore.GetItemAsync(itemId);
                Id = item.Id;
                Text = item.Text;
                Description = item.Artist;
            }
            catch (Exception)
            {
                Debug.WriteLine("Failed to Load Item");
            }
        }
    }
}
