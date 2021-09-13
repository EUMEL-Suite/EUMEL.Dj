namespace Eumel.Dj.WebServer.Logging
{
    public class SyslogSettings : ISyslogSettings
    {
        public bool EnableSyslogLogging { get; set; }

        public string SysLogServerIp { get; set; }

        public int SyslogServerPort { get; set; }

        public string CertificatePath { get; set; }

        public string CertificatePassword { get; set; }
        public string MinimumLevel { get; set; }
        public bool UseUdp { get; set; }
    }
}