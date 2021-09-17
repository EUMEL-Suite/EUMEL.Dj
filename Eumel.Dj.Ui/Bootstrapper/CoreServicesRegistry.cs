using Accessibility;
using Caliburn.Micro;
using Eumel.Dj.Core.Models;
using StructureMap;
using TinyMessenger;

namespace Eumel.Dj.Ui.Bootstrapper
{
    public class CoreServicesRegistry : Registry
    {
        public CoreServicesRegistry()
        {
            _ = For<IWindowManager>().Use(new WindowManager());
            _ = For<ITinyMessengerHub>().Use(TinyMessengerHub.DefaultHub);

            var appSettings = new AppSettings
            {
                // TODO SUCKS
                RestEndpoint = "https://192.168.178.37:443",
                SyslogServer = "192.168.178.37",
                MinimumLogLevel = Constants.EumelLogLevel.Information
            };
            _ = For<IAppSettings>().Use(appSettings);
            _ = For<IWebServiceHost>().Use<WebServiceHost>().Singleton();
        }
    }
}