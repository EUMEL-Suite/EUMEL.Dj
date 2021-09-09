namespace Eumel.Dj.Mobile.Services
{
    public interface ISettingsService
    {
        string RestEndpoint { get; }
        string Username { get; }
        string SyslogServer { get; set; }
    }
}