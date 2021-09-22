using System;
using System.IO;
using Accessibility;
using Caliburn.Micro;
using Eumel.Dj.Core.Logging;
using Eumel.Dj.Core.Models;
using Eumel.Dj.Ui.AutoStartServices;
using Eumel.Dj.Ui.Services;
using StructureMap;
using TinyMessenger;

namespace Eumel.Dj.Ui.Bootstrapper
{
    public class CoreServicesRegistry : Registry
    {
        public CoreServicesRegistry()
        {
            var appSettings = AppSettingsExtensions.FromFile(Environment.ExpandEnvironmentVariables(Constants.EumelSuiteAppData) + "\\" + Constants.ApplicationName + ".settings.json");

            _ = For<IWindowManager>().Use(new WindowManager());
            _ = For<ITinyMessengerHub>().Use(TinyMessengerHub.DefaultHub);

            _ = For<ILoggerFactory>().Use<SerilogFactory>();


            _ = For<IAppSettings>().Use(appSettings);
            _ = For<ILoggerSettings>().Use(appSettings.LoggerSettings);
            _ = For<IEumelLogger>().Use((c) => c.GetInstance<ILoggerFactory>().Build(c.GetInstance<ILoggerSettings>()));

            //var appSettings = new AppSettings
            //{
            //    // TODO SUCKS
            //    RestEndpoint = "https://192.168.178.37:443",
            //    SyslogServer = "192.168.178.37",
            //    MinimumLogLevel = Constants.EumelLogLevel.Information
            //};


            _ = For<IAutoStart>().Use<WebServiceHost>().Singleton();
            _ = For<IAutoStart>().Use<DjService>().Singleton();
            _ = For<IAutoStart>().Use<UserService>().Singleton();
            _ = For<IAutoStart>().Use<LoggingService>().Singleton();

            _ = For<IPlaylistProviderService>().Use<ItunesProviderService>().Singleton();

        }
    }
}