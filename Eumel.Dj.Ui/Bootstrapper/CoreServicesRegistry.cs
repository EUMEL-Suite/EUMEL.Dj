using System;
using Caliburn.Micro;
using Eumel.Dj.Core;
using Eumel.Dj.Core.Logging;
using Eumel.Dj.Core.Models;
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
            _ = For(typeof(IImplementationResolver<>)).Use(typeof(ImplementationResolver<>)).Singleton();


            // settings and logger added to container
            var appSettings = AppSettingsExtensions.FromFile(Environment.ExpandEnvironmentVariables(Constants.EumelSuiteAppData) + "\\" + Constants.ApplicationName + ".settings.json");
            _ = For<IAppSettings>().Use(appSettings);
            _ = For<ILoggerSettings>().Use(appSettings.LoggerSettings);
            _ = For<IImplementationSettings>().Use(appSettings.ImplementationSettings);

            // register the logger
            _ = For<ILoggerFactory>().Use<SerilogFactory>();
            _ = For<IEumelLogger>().Use((c) => c.GetInstance<ILoggerFactory>().Build(c.GetInstance<ILoggerSettings>()));
        }
    }
}