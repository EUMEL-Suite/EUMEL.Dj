using SyslogLogging;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Eumel.Dj.Mobile.Services
{
    public class SyslogService : ISyslogService
    {
        private ISettingsService _settings => DependencyService.Get<ISettingsService>();

        private LoggingModule _syslogger;
        private readonly string _deviceName = DeviceInfo.Name;

        private LoggingModule GetLogger()
        {
            if (_syslogger != null) return _syslogger;
            if (string.IsNullOrEmpty(_settings?.SyslogServer))
                return null;

            _syslogger = new LoggingModule(_settings.SyslogServer, 514);
            _syslogger.Settings.HeaderFormat = "{ts}\t" + _deviceName.PadRight(18).Substring(0, 18) + "\t{sev}\t";
            _syslogger.Info($"Syslogger started on device {_deviceName}");
            return _syslogger;
        }

        public void Debug(string msg)
        {
            if (_settings.MinimumLogLevel == EumelLogLevel.Debug)

                GetLogger()?.Debug(msg);
        }

        public void Information(string msg)
        {
            if (_settings.MinimumLogLevel == EumelLogLevel.Information ||
                _settings.MinimumLogLevel == EumelLogLevel.Debug)

                GetLogger()?.Info(msg);
        }

        public void Warn(string msg)
        {
            if (_settings.MinimumLogLevel == EumelLogLevel.Warn ||
                _settings.MinimumLogLevel == EumelLogLevel.Information ||
                _settings.MinimumLogLevel == EumelLogLevel.Debug)

                GetLogger()?.Warn(msg);
        }

        public void Error(string msg)
        {
            if (_settings.MinimumLogLevel == EumelLogLevel.Error ||
                _settings.MinimumLogLevel == EumelLogLevel.Warn ||
                _settings.MinimumLogLevel == EumelLogLevel.Information ||
                _settings.MinimumLogLevel == EumelLogLevel.Debug)

                GetLogger()?.Error(msg);
        }

        public void Fatal(string msg)
        {
            if (_settings.MinimumLogLevel == EumelLogLevel.Fatal ||
                _settings.MinimumLogLevel == EumelLogLevel.Error ||
                _settings.MinimumLogLevel == EumelLogLevel.Warn ||
                _settings.MinimumLogLevel == EumelLogLevel.Information ||
                _settings.MinimumLogLevel == EumelLogLevel.Debug)

                GetLogger()?.Critical(msg);
        }
    }
}