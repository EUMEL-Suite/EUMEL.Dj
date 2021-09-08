using System;
using System.Diagnostics;
using Eumel.Dj.Mobile.Models;
using Eumel.Dj.Mobile.Services;
using Xamarin.Forms;

namespace Eumel.Dj.Mobile.ViewModels
{
    [QueryProperty(nameof(ItemId), nameof(ItemId))]
    public class SongDetailViewModel : BaseViewModel
    {
        public ISongService SongStore => DependencyService.Get<ISongService>();

        private string itemId;
        private string text;
        private string description;
        private string hasMyVote;
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

        public string HasMyVote
        {
            get => hasMyVote;
            set => SetProperty(ref hasMyVote, value);
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

        public async void LoadItemId(string id)
        {
            try
            {
                var item = await SongStore.GetItemAsync(id);
                Id = item.Id;
                Text = item.Title;
                Description = item.Description;
                HasMyVote = item.HasMyVote ? "Yes" : "No";
            }
            catch (Exception)
            {
                Debug.WriteLine("Failed to Load Item");
            }
        }
    }
}
