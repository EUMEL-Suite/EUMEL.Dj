using Eumel.Dj.Mobile.ViewModels;
using Xamarin.Forms;

namespace Eumel.Dj.Mobile.Views
{
    public partial class PlaylistPage : ContentPage
    {
        PlaylistViewModel _viewModel;
        public PlaylistPage()
        {
            InitializeComponent();
            BindingContext = _viewModel = new PlaylistViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnAppearing();
        }
    }
}