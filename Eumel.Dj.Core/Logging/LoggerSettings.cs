namespace Eumel.Dj.Core.Logging
{
    public class LoggerSettings : ILoggerSettings
    {
        public SyslogSettings Syslog { get; set; }
        public FilelogSettings Filelog { get; set; }
        public string DeviceName { get; set; }
    }
}