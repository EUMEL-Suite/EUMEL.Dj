using System;
using Caliburn.Micro;
using Castle.DynamicProxy;
using Eumel.Core.Logging;
using Eumel.Dj.Core;
using Eumel.Dj.Core.Models;
using Eumel.Logging.Serilog;
using StructureMap;
using TinyMessenger;

namespace Eumel.Dj.Ui.DependencyInjection
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
            _ = For<LoggerSettings>().Use(appSettings.LoggerSettings);
            _ = For<ImplementationSettings>().Use(appSettings.ImplementationSettings);

            // register the logger
            _ = For<ILoggerFactory>().Use<SerilogFactory>().Singleton();
            _ = For<IEumelLogger>().Use((c) => c.GetInstance<ILoggerFactory>().Build(c.GetInstance<LoggerSettings>()));
            EnableInterceptor(appSettings.LoggerSettings, new SerilogFactory().Build(appSettings.LoggerSettings));
            Policies.Interceptors(new ActivationTraceInterceptor(new SerilogFactory().Build(appSettings.LoggerSettings)));
        }

        private void EnableInterceptor(LoggerSettings settings, IEumelLogger logger)
        {
            if (!settings.EnableInterceptor) return;

            var proxyGenerator = new ProxyGenerator();
            var loggingInterceptor = new LoggingInterceptor(logger);

            // currently there is no (easy) generic way to decorate ALL registered interfaces
            // the proxy generator wraps the interface and uses the logging interceptor to decorate the call
            For<ITinyMessengerHub>().DecorateAllWith(targetInterface => proxyGenerator.CreateInterfaceProxyWithTargetInterface(targetInterface, loggingInterceptor));
        }
    }
}