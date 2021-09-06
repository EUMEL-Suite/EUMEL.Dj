namespace Eumel.Dj.WebServer.Logging
{
    public interface ILoggerSettings
    {
        ISyslogSettings Syslog { get; set; }
        IFilelogSettings Filelog { get; set; }
    }
}