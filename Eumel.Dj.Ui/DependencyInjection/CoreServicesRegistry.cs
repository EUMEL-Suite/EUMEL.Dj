using System;
using System.Diagnostics;
using Caliburn.Micro;
using Castle.DynamicProxy;
using Eumel.Core.Logging;
using Eumel.Dj.Core;
using Eumel.Dj.Core.Models;
using Eumel.Logging;
using Eumel.Logging.Serilog;
using StructureMap;
using TinyMessenger;
using ILoggerFactory = Eumel.Core.Logging.ILoggerFactory;

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
            _ = For<IImplementationSettings>().Use(appSettings.ImplementationSettings);

            // register the logger
            _ = For<ILoggerFactory>().Use<SerilogFactory>();
            _ = For<IEumelLogger>().Use((c) => c.GetInstance<ILoggerFactory>().Build(c.GetInstance<LoggerSettings>()));

            var proxyGenerator = new ProxyGenerator();
            var loggingInterceptor = new LoggingInterceptor(new EumelConsoleLogger(appSettings.LoggerSettings));

            For<ITinyMessengerHub>().DecorateAllWith(targetInterface =>
                proxyGenerator.CreateInterfaceProxyWithTargetInterface(targetInterface, loggingInterceptor));
        }
    }


    public class LoggingInterceptor : Castle.DynamicProxy.IInterceptor
    {
        private readonly IEumelLogger _logger;

        public LoggingInterceptor(IEumelLogger logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public void Intercept(IInvocation invocation)
        {
            var watch = Stopwatch.StartNew();
            var enteringLogMessage = "Entering Method: " + invocation.Method.Name;

            if (invocation.Arguments.Length > 0)
                enteringLogMessage += " with Arguments: " + string.Join(",", invocation.Arguments);
            _logger.Verbose(enteringLogMessage);

            try
            {
                //Run the actual Invocation
                invocation.Proceed();
            }
            catch (Exception ex)
            {
                var exceptionLogMessage = "Caught Exception of type: " + ex.GetType() + " in Method: " +
                                          invocation.Method.Name;
                _logger.Error(exceptionLogMessage, ex);
                throw;
            }
            finally
            {
                watch.Stop();
                _logger.Verbose("Leaving: " + invocation.Method.Name + " after " + watch.ElapsedMilliseconds +
                                " Milliseconds");
            }
        }
    }
}