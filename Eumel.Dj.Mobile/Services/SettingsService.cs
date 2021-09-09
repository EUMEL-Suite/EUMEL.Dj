using Xamarin.Essentials;

namespace Eumel.Dj.Mobile.Services
{
    public class SettingsService : ISettingsService
    {
        public SettingsService()
        {
            Username = Preferences.Get("Eumel.Dj.Username", "Jane Doe");
        }

        public string RestEndpoint { get; }
        public string Username { get; }
        public string SyslogServer { get; set; } = "192.168.178.37";
    }
}