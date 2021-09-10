using Eumel.Dj.Mobile.Services;
using Eumel.Dj.Mobile.ViewModels;
using Xamarin.Forms;

namespace Eumel.Dj.Mobile.Views
{
    public partial class PlaylistPage : ContentPage
    {
        private readonly PlaylistViewModel _viewModel;

        public PlaylistPage()
        {
            InitializeComponent();
            BindingContext = _viewModel = new PlaylistViewModel();

            // if no endpoint set, navigate to login page
            if (string.IsNullOrWhiteSpace(DependencyService.Get<ISettingsService>().RestEndpoint))
                Shell.Current.GoToAsync("//Login");
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnAppearing();
        }
    }
}