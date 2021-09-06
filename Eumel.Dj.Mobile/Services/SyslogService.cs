using SyslogLogging;

namespace Eumel.Dj.Mobile.Services
{
    public class SyslogService : ISyslogService
    {
        private readonly LoggingModule _log;

        public SyslogService()
        {
            _log = new LoggingModule("192.168.178.37", 514);
            _log.Debug("This is a debug message!");
        }

        public void Debug(string msg)
        {
            _log.Debug(msg);
        }
    }
}