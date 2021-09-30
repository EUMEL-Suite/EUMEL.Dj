using System;

namespace Eumel.Core.Logging
{
    public static class LoggerSettingsExtensions
    {
        private const string VerboseLiteral = "Verbose";

        public static bool AllLoggersAreDisabled(this LoggerSettings settings)
        {
            if (settings == null) return true;

            var oneLoggerIsEnabled =
                (settings.Filelog?.EnableFileLogging ?? false) ||
                (settings.Syslog?.EnableSyslogLogging ?? false) || settings.UseConsole || settings.UseDebug;

            return !oneLoggerIsEnabled;
        }

        public static bool AtLeastOneVerbose(this LoggerSettings settings)
        {
            return string.Compare(settings.Filelog?.MinimumLevel, VerboseLiteral, StringComparison.InvariantCultureIgnoreCase) == 0 ||
                   string.Compare(settings.Syslog?.MinimumLevel, VerboseLiteral, StringComparison.InvariantCultureIgnoreCase) == 0;
        }
    }
}