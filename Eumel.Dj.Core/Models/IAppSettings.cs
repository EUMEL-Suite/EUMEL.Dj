namespace Eumel.Dj.WebServer.Models
{
    public interface IAppSettings
    {
        string RestEndpoint { get; set; }
        string SyslogServer { get; set; }
        Constants.EumelLogLevel MinimumLogLevel { get; set; }
    }
}