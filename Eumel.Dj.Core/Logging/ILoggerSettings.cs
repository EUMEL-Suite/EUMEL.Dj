namespace Eumel.Dj.WebServer.Logging
{
    public interface ILoggerSettings
    {
        ISyslogSettings Syslog { get; set; }
        IFilelogSettings Filelog { get; set; }
        string DeviceName { get; set; }
    }
}