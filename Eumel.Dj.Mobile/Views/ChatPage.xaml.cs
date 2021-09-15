using Eumel.Dj.Mobile.ViewModels;
using Xamarin.Forms;

namespace Eumel.Dj.Mobile.Views
{
    public partial class ChatPage : ContentPage
    {
        private readonly ChatViewModel _viewModel;

        public ChatPage()
        {
            InitializeComponent();
            BindingContext = _viewModel = new ChatViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnAppearing();
        }
    }
}