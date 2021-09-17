namespace Eumel.Dj.Core.Models
{
    public class AppSettings : IAppSettings
    {
        public string RestEndpoint { get; set; }
        public string SyslogServer { get; set; }
        public Constants.EumelLogLevel MinimumLogLevel { get; set; }
    }
}