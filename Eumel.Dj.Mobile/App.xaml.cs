using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Eumel.Dj.Mobile.Services;
using Eumel.Dj.Mobile.Views;
using Plugin.Connectivity;
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

            // check if endpoint is available
            //if (!string.IsNullOrWhiteSpace(DependencyService.Get<ISettingsService>().RestEndpoint))
            //{
            //    var s = new Stopwatch();
            //    s.Start();
            //    try
            //    {
            //        var cts = new CancellationTokenSource(TimeSpan.FromSeconds(3));
            //        cts.CancelAfter(TimeSpan.FromSeconds(10));
            //        var tokenIsValid = DependencyService.Get<IEumelRestServiceFactory>().Build().TokenIsValidAsync(cts.Token).Result;
            //        s.Stop();
            //        if (!tokenIsValid)
            //        {
            //            DependencyService.Get<ISyslogService>().Debug("Token is not valid. Reset settings.");
            //            //DependencyService.Get<ISettingsService>().Reset();
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        s.Stop();
            //        DependencyService.Get<ISyslogService>().Debug("Exception thrown while token validation. Reset settings." + ex.Message);
            //        //DependencyService.Get<ISettingsService>().Reset();
            //    }
            //    finally
            //    {
            //        s.Stop();
            //        Console.WriteLine(s.ElapsedMilliseconds / 1000);
            //    }
            //}



            if (!IsPingable(DependencyService.Get<ISettingsService>()).Result)
            {
                DependencyService.Get<ISyslogService>().Debug("Endpoint not available (ping). Resetting settings and force login.");

                //DependencyService.Get<ISettingsService>().Reset();
            }

            if (!IsReachableAndRunning(DependencyService.Get<ISettingsService>()).Result)
            {
                DependencyService.Get<ISyslogService>().Debug("Service endpoint not available. Resetting settings and force login.");

                //DependencyService.Get<ISettingsService>().Reset();
            }

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

        public async Task<bool> IsReachableAndRunning(ISettingsService settings)
        {
            if (string.IsNullOrEmpty(settings.RestEndpoint))
                return await Task.FromResult(false);

            var connectivity = CrossConnectivity.Current;
            if (!connectivity.IsConnected)
                return false;

            var uri = new Uri(settings.RestEndpoint);
            var reachable = await connectivity.IsRemoteReachable(uri.Host, uri.Port);

            return reachable;
        }

        public async Task<bool> IsPingable(ISettingsService settings)
        {
            if (string.IsNullOrEmpty(settings.RestEndpoint))
                return await Task.FromResult(false);

            var connectivity = CrossConnectivity.Current;
            if (!connectivity.IsConnected)
                return false;

            var uri = new Uri(settings.RestEndpoint);
            var reachable = await connectivity.IsReachable(uri.Host);

            return reachable;
        }
    }
}