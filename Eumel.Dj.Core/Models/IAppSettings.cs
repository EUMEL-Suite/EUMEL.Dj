using Eumel.Core.Logging;

namespace Eumel.Dj.Core.Models
{
    public interface IAppSettings
    {
        string RestEndpoint { get; set; }
        string SyslogServer { get; set; }
        Constants.EumelLogLevel ClientLogLevel { get; set; }
        string ItunesLibrary { get; }
        string SelectedPlaylist { get; }
        LoggerSettings LoggerSettings { get; set; }
        string SongsPath { get; set; }
        int Port { get; set; }
    }
}