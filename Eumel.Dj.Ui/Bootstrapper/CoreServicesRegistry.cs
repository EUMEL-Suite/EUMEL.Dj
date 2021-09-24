using System;
using Caliburn.Micro;
using Eumel.Dj.Core.Logging;
using Eumel.Dj.Core.Models;
using Eumel.Dj.Ui.Extensions.Apple;
using Eumel.Dj.Ui.Services;
using StructureMap;
using TinyMessenger;

namespace Eumel.Dj.Ui.Bootstrapper
{
    public class CoreServicesRegistry : Registry
    {
        public CoreServicesRegistry()
        {
            // we need to register some core infrastructure
            _ = For<IWindowManager>().Use(new WindowManager());
            _ = For<ITinyMessengerHub>().Use(TinyMessengerHub.DefaultHub);


            // settings and logger added to container
            var appSettings = AppSettingsExtensions.FromFile(Environment.ExpandEnvironmentVariables(Constants.EumelSuiteAppData) + "\\" + Constants.ApplicationName + ".settings.json");
            _ = For<IAppSettings>().Use(appSettings);
            _ = For<ILoggerSettings>().Use(appSettings.LoggerSettings);

            _ = For<ILoggerFactory>().Use<SerilogFactory>();
            _ = For<IEumelLogger>().Use((c) => c.GetInstance<ILoggerFactory>().Build(c.GetInstance<ILoggerSettings>()));


            // now we have the plugins and extensions which are registered
            // todo: use a class which takes config and implementations
            _ = For<IPlaylistProviderService>().Use<ItunesProviderService>().Singleton();
        }
    }
}