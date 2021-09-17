namespace Eumel.Dj.Core.Models
{
    public class ServerSettings
    {
        public string SyslogServer { get; set; }
        public Constants.EumelLogLevel MinimumLogLevel { get; set; }
        public string Username { get; set; }
        public string Usertoken { get; set; }
    }
}