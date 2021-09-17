namespace Eumel.Dj.Core.Logging
{
    public interface ILoggerSettings
    {
        ISyslogSettings Syslog { get; set; }
        IFilelogSettings Filelog { get; set; }
        string DeviceName { get; set; }
    }
}