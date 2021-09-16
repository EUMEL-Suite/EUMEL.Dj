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

            // for testing to delete settings
            DependencyService.Get<ISettingsService>().Reset();

            // if no endpoint set, navigate to login page
            if (string.IsNullOrWhiteSpace(DependencyService.Get<ISettingsService>().RestEndpoint))
            {
                // the message will never appear because syslog is unknown!
                DependencyService.Get<ISyslogService>().Debug("Application not configured. Loading login page.");
                MainPage = new LoginPage();
            }
            else
            {
                DependencyService.Get<ISyslogService>().Debug("Application already configured with endpoint. Starting AppShell.");
                MainPage = new AppShell();
            }
        }
    }
}