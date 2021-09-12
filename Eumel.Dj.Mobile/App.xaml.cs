using Eumel.Dj.Mobile.Services;
using Xamarin.Essentials;
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

            DependencyService.Get<ISyslogService>().Information("Starting application");

            MainPage = new AppShell();
        }
    }
}
