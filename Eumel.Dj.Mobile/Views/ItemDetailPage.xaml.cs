using System;
using Eumel.Dj.Mobile.ViewModels;
using System.Net.Http;
using Xamarin.Forms;

namespace Eumel.Dj.Mobile.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        public ItemDetailPage()
        {
            InitializeComponent();
            BindingContext = new ItemDetailViewModel();
        }

        private void Button_OnClicked(object sender, EventArgs e)
        {
            var current = (ItemDetailViewModel)BindingContext;
            var cl = new HttpClientHandler();
            cl.ServerCertificateCustomValidationCallback = (message, certificate2, arg3, arg4) => true;
            var client = new HttpClient(cl);

            var svc = new EumelDjServiceClient("https://192.168.178.37:443", client);
            svc.OpenAsync(current.Location);
        }
    }
}