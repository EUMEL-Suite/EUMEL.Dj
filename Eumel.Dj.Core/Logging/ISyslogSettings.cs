namespace Eumel.Dj.Core.Logging
{
    public interface ISyslogSettings
    {
        bool EnableSyslogLogging { get; set; }
        string SysLogServerIp { get; set; }
        int SyslogServerPort { get; set; }
        string CertificatePath { get; set; }
        string CertificatePassword { get; set; }
        string MinimumLevel { get; set; }
        bool UseUdp { get; set; }
    }
}