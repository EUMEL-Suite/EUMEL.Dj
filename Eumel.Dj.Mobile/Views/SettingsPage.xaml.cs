using Eumel.Dj.Mobile.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Eumel.Dj.Mobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingsPage : ContentPage
    {

        private readonly SettingsViewModel _viewModel;

        public SettingsPage()
        {
            InitializeComponent();
            BindingContext = _viewModel = new SettingsViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnAppearing();
        }
    }
}