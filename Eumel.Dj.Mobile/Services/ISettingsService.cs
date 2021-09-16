using System.Threading.Tasks;

namespace Eumel.Dj.Mobile.Services
{
    public interface ISettingsService
    {
        string RestEndpoint { get; }
        string Username { get; }
        string Token { get; }
        string SyslogServer { get; }
        EumelLogLevel MinimumLogLevel { get; }
        void Change(string restEndpoint, string username, string syslogServer, string token, EumelLogLevel minimumLogLevel);
        void Reset();
        Task<bool> CheckUserIsAdmin();
        Task<bool> TokenIsInvalid();
        Task Logout();
    }
}