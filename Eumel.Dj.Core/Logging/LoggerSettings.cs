﻿namespace Eumel.Dj.Core.Logging
{
    public class LoggerSettings : ILoggerSettings
    {
        public ISyslogSettings Syslog { get; set; }
        public IFilelogSettings Filelog { get; set; }
        public string DeviceName { get; set; }
    }
}