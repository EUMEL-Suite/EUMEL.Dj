using Eumel.Dj.Mobile.Views;
using System.Net;
using Eumel.Dj.Mobile.Services;
using Xamarin.Forms;

namespace Eumel.Dj.Mobile
{
    public partial class AppShell
    {
        public AppShell()
        {
            InitializeComponent();

            //ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;

            Routing.RegisterRoute(nameof(PlaylistPage), typeof(PlaylistPage));
            Routing.RegisterRoute(nameof(SongsPage), typeof(SongsPage));
            Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
            Routing.RegisterRoute(nameof(SettingsPage), typeof(SettingsPage));
        }
    }
}
