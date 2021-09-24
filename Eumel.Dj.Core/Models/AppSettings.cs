using Eumel.Dj.Core.Logging;

namespace Eumel.Dj.Core.Models
{
    public class AppSettings : IAppSettings
    {
        public string ItunesLibrary { get; set; }
        public string SelectedPlaylist { get; set; }
        public string RestEndpoint { get; set; }
        public string SyslogServer { get; set; }
        public Constants.EumelLogLevel MinimumLogLevel { get; set; }
        public LoggerSettings LoggerSettings { get; set; }
        public string SongsPath { get; set; }
    }
}