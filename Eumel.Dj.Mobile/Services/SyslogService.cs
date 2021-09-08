using SyslogLogging;

namespace Eumel.Dj.Mobile.Services
{
    public class SyslogService : ISyslogService
    {
        private readonly LoggingModule _log;

        public SyslogService()
        {
            _log = new LoggingModule("192.168.178.37", 514);
            _log.Settings.HeaderFormat = "{ts}\t{host}\t{thread}\t{sev}\t";
        }

        public void Debug(string msg)
        {
            _log.Debug(msg);
        }

        public void Information(string msg)
        {
            _log.Info(msg);
        }

        public void Error(string msg)
        {
            _log.Error(msg);
        }
    }
}