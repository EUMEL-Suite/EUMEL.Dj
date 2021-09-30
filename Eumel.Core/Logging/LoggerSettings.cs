namespace Eumel.Core.Logging
{
    public class LoggerSettings
    {
        public SyslogSettings Syslog { get; set; }
        public FilelogSettings Filelog { get; set; }
        public bool UseConsole { get; set; }
        public bool UseDebug { get; set; }
        public bool UseExtendedDebug { get; set; }
    }
}