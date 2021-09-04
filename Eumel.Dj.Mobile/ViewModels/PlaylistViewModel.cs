using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Eumel.Dj.Mobile.ViewModels
{
    public class PlaylistViewModel : BaseViewModel
    {
        public PlaylistViewModel()
        {
            Title = "Playlist";
            OpenWebCommand = new Command(async () => await Browser.OpenAsync("https://aka.ms/xamarin-quickstart"));
        }

        public ICommand OpenWebCommand { get; }

        public ICommand VoteUpDown { get; }
    }
}