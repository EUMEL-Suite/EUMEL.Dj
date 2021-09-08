using System.Windows;
using Eumel.Dj.Ui.Services;
using Eumel.Dj.WebServer.Logging;
using TinyMessenger;

namespace Eumel.Dj.Ui
{
    /// <summary>
    ///     Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private TinyMessengerHub _hub;
        private LoggingService _loghubService;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var loggerSettings = new LoggerSettings()
            {
                Filelog = new FilelogSettings() { EnableFileLogging = false },
                Syslog = new SyslogSettings() { EnableSyslogLogging = true, SysLogServerIp = "192.168.178.37", UseUdp = true }
            };
            var logger = new SerilogFactory().Build(loggerSettings);

            _hub = TinyMessengerHub.DefaultHub;
            _loghubService = new LoggingService(_hub, logger);
        }
    }
}