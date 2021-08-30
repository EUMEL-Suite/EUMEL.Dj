using Eumel.Dj.Mobile.Models;
using Eumel.Dj.Mobile.ViewModels;
using System.Net.Http;
using Xamarin.Forms;

namespace Eumel.Dj.Mobile.Views
{
    public partial class NewItemPage : ContentPage
    {
        public Item Item { get; set; }

        public NewItemPage()
        {
            InitializeComponent();
            BindingContext = new NewItemViewModel();

            var cl = new HttpClientHandler();
            cl.ServerCertificateCustomValidationCallback = (message, certificate2, arg3, arg4) => true;
            var client = new HttpClient(cl);

            var svc = new swaggerClient("https://192.168.178.37:443", client);
             svc.PlaylistAsync("awesome mobile message");
        }
    }
}