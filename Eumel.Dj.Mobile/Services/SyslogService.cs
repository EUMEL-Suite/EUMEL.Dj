using SyslogLogging;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Eumel.Dj.Mobile.Services
{
    public class SyslogService : ISyslogService
    {
        private ISettingsService _settings => DependencyService.Get<ISettingsService>();

        private LoggingModule _syslogger;

        private LoggingModule GetLogger()
        {
#if RELEASE
            return null;
#endif

            if (_syslogger != null) return _syslogger;
            // this makes sure we don't send information to non-set server
            if (string.IsNullOrEmpty(_settings?.SyslogServer))
                return null;
            var deviceName = _settings.Username ?? "<anonymous>";
            _syslogger = new LoggingModule(_settings.SyslogServer, 514);
            _syslogger.Settings.HeaderFormat = "{ts}\t" + deviceName.PadRight(18).Substring(0, 18) + "\t{sev}\t";
            _syslogger.Info($"Syslogger started on device {deviceName}");
            return _syslogger;
        }

        public void Verbose(string msg)
        {
            if (_settings.MinimumLogLevel == EumelLogLevel.Verbose)

                GetLogger()?.Debug(msg);
        }

        public void Debug(string msg)
        {
            if (_settings.MinimumLogLevel == EumelLogLevel.Debug ||
                _settings.MinimumLogLevel == EumelLogLevel.Verbose)

                GetLogger()?.Debug(msg);
        }

        public void Information(string msg)
        {
            if (_settings.MinimumLogLevel == EumelLogLevel.Information ||
                _settings.MinimumLogLevel == EumelLogLevel.Debug ||
                _settings.MinimumLogLevel == EumelLogLevel.Verbose)

                GetLogger()?.Info(msg);
        }

        public void Warn(string msg)
        {
            if (_settings.MinimumLogLevel == EumelLogLevel.Warn ||
                _settings.MinimumLogLevel == EumelLogLevel.Information ||
                _settings.MinimumLogLevel == EumelLogLevel.Debug ||
                _settings.MinimumLogLevel == EumelLogLevel.Verbose)

                GetLogger()?.Warn(msg);
        }

        public void Error(string msg)
        {
            if (_settings.MinimumLogLevel == EumelLogLevel.Error ||
                _settings.MinimumLogLevel == EumelLogLevel.Warn ||
                _settings.MinimumLogLevel == EumelLogLevel.Information ||
                _settings.MinimumLogLevel == EumelLogLevel.Debug ||
                _settings.MinimumLogLevel == EumelLogLevel.Verbose)

                GetLogger()?.Error(msg);
        }

        public void Fatal(string msg)
        {
            if (_settings.MinimumLogLevel == EumelLogLevel.Fatal ||
                _settings.MinimumLogLevel == EumelLogLevel.Error ||
                _settings.MinimumLogLevel == EumelLogLevel.Warn ||
                _settings.MinimumLogLevel == EumelLogLevel.Information ||
                _settings.MinimumLogLevel == EumelLogLevel.Debug ||
                _settings.MinimumLogLevel == EumelLogLevel.Verbose)

                GetLogger()?.Critical(msg);
        }
    }
}