using System.Windows;
using Eumel.Dj.Ui.Services;
using Eumel.Dj.WebServer.Logging;
using TinyMessenger;

namespace Eumel.Dj.Ui
{
    public partial class App : Application
    {
        // ReSharper disable once NotAccessedField.Local
        private LoggingService _loggingService;
        private TinyMessengerHub _hub;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // todo: there is a class with system settings we needs to inject
            var loggerSettings = new LoggerSettings()
            {
                Filelog = new FilelogSettings() { EnableFileLogging = false },
                Syslog = new SyslogSettings() { EnableSyslogLogging = true, SysLogServerIp = "192.168.178.37", UseUdp = true }
            };
            var logger = new SerilogFactory().Build(loggerSettings);

            _hub = TinyMessengerHub.DefaultHub;
            _loggingService = new LoggingService(_hub, logger);
        }
    }
}