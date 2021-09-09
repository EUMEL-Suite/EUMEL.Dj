using Eumel.Dj.Mobile.ViewModels;
using Xamarin.Forms;

namespace Eumel.Dj.Mobile.Views
{
    public partial class SongsPage : ContentPage
    {
        private readonly SongsViewModel _viewModel;

        public SongsPage()
        {
            InitializeComponent();

            BindingContext = _viewModel = new SongsViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnAppearing();
        }
    }
}