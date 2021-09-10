using SyslogLogging;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Eumel.Dj.Mobile.Services
{
    public class SyslogService : ISyslogService
    {
        private readonly ISettingsService _settings;
        private LoggingModule _syslogger;
        private readonly string _deviceName;

        public SyslogService()
        {
            _settings = DependencyService.Get<ISettingsService>();
            _deviceName = DeviceInfo.Name;
        }

        private LoggingModule GetLogger()
        {
            if (_syslogger != null) return _syslogger;
            if (string.IsNullOrEmpty(_settings?.SyslogServer))
                return null;

            _syslogger = new LoggingModule(_settings.SyslogServer, 514);
            _syslogger.Settings.HeaderFormat = "{ts}\t" + _deviceName + "\t{sev}\t";
            _syslogger.Info($"Syslogger started on device {_deviceName}");
            return _syslogger;
        }

        public void Debug(string msg)
        {
            GetLogger()?.Debug(msg);
        }

        public void Information(string msg)
        {
            GetLogger()?.Info(msg);
        }

        public void Error(string msg)
        {
            GetLogger()?.Error(msg);
        }
    }
}