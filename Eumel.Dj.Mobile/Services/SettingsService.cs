namespace Eumel.Dj.Mobile.Services
{
    public class SettingsService : ISettingsService
    {
        public string RestEndpoint { get; } = "https://192.168.178.37:443";
        public string Username { get; } = "Thomas";
    }
}