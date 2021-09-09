namespace Eumel.Dj.WebServer.Models
{
    public interface IAppSettings
    {
        string RestEndpoint { get; set; }
        string SyslogServer { get; set; }
        string MinimumLogLevel { get; set; }
    }

    public class AppSettings : IAppSettings
    {
        public string RestEndpoint { get; set; }
        public string SyslogServer { get; set; }
        public string MinimumLogLevel { get; set; }
    }
}