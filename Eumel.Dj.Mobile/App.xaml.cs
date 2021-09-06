using System;
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

            DependencyService.Register<SongItemSongStore>();

            MainPage = new AppShell();
        }

        protected override void OnStart()
        {
            var logger = DependencyService.Get<ISyslogService>();
            logger.Debug($"[{DeviceInfo.Name}] App started");
        }

        protected override void OnSleep()
        {
            var logger = DependencyService.Get<ISyslogService>();
            logger.Debug($"[{DeviceInfo.Name}] App going to sleep");
        }

        protected override void OnResume()
        {
            var logger = DependencyService.Get<ISyslogService>();
            logger.Debug($"[{DeviceInfo.Name}] App going to resume");
        }
    }
}
