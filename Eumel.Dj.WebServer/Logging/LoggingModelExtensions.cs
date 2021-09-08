namespace Eumel.Dj.WebServer.Logging
{
    public static class LoggingModelExtensions
    {
        public static bool AllLoggersAreDisabled(this ILoggerSettings settings)
        {
            if (settings == null) return true;

            var oneLoggerIsEnabled =
                (settings.Filelog?.EnableFileLogging ?? false) ||
                (settings.Syslog?.EnableSyslogLogging ?? false);

            return !oneLoggerIsEnabled;
        }
    }
}