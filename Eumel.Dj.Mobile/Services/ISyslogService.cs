namespace Eumel.Dj.Mobile.Services
{
    public interface ISyslogService
    {
        void Debug(string msg);
        void Information(string msg);
        void Error(string msg);
        void Warn(string msg);
        void Fatal(string msg);
    }
}