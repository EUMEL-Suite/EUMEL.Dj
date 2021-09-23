using Eumel.Dj.Core.Logging;

namespace Eumel.Dj.Core.Models
{
    public interface IAppSettings
    {
        string RestEndpoint { get; set; }
        string SyslogServer { get; set; }
        Constants.EumelLogLevel MinimumLogLevel { get; set; }
        string ItunesLibrary { get; }
        string SelectedPlaylist { get; }
        LoggerSettings LoggerSettings { get; set; }
    }
}