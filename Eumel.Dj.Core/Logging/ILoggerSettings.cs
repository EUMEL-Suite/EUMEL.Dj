namespace Eumel.Dj.Core.Logging
{
    public interface ILoggerSettings
    {
        SyslogSettings Syslog { get; }
        FilelogSettings Filelog { get; }
        string DeviceName { get; set; }
    }
}