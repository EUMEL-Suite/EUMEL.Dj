namespace Eumel.Dj.Mobile.Services
{
    public interface ISyslogService
    {
        void Debug(string msg);
        void Information(string msg);
        void Error(string msg);
    }
}