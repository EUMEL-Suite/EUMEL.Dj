using Eumel.Dj.Mobile.Views;
using System.Net;
using Xamarin.Forms;

namespace Eumel.Dj.Mobile
{
    public partial class AppShell
    {
        public AppShell()
        {
            InitializeComponent();

            ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;

            Routing.RegisterRoute(nameof(PlaylistPage), typeof(PlaylistPage));
            Routing.RegisterRoute(nameof(SongsPage), typeof(SongsPage));
            Routing.RegisterRoute(nameof(ItemDetailPage), typeof(ItemDetailPage));
        }
    }
}
