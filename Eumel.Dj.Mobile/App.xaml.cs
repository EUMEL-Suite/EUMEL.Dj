using Eumel.Dj.Mobile.Services;
using Eumel.Dj.Mobile.Views;
using Xamarin.Forms;

namespace Eumel.Dj.Mobile
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            DependencyService.Register<SyslogService>();
            DependencyService.Register<SettingsService>();
            DependencyService.Register<EumelRestServiceFactory>();
            DependencyService.Register<RestPlaylistService>();
            DependencyService.Register<RestSongService>();
            DependencyService.Register<RestPlayerService>();
            DependencyService.Register<RestChatService>();

            // if no endpoint set, navigate to login page
            MainPage = string.IsNullOrWhiteSpace(DependencyService.Get<ISettingsService>().RestEndpoint)
                ? (Page)new LoginPage()
                : new AppShell();
        }
    }
}