using Eumel.Dj.Mobile.Data;
using Xamarin.Essentials;

namespace Eumel.Dj.Mobile.Services
{
    public class SettingsService : ISettingsService
    {
        private const string SettingPrefix = "Eumel.Dj.";
        public SettingsService()
        {
            Username = Preferences.Get(SettingPrefix + nameof(Username), Marvel.Names.Random());
            RestEndpoint = Preferences.Get(SettingPrefix + nameof(RestEndpoint), null);
            SyslogServer = Preferences.Get(SettingPrefix + nameof(SyslogServer), null);
            Token = Preferences.Get(SettingPrefix + nameof(Token), null);

            if (string.IsNullOrWhiteSpace(Username))
                Username = Marvel.Names.Random();
        }

        public void Change(string restEndpoint, string username, string syslogServer, string token)
        {
            RestEndpoint = restEndpoint;
            Username = username;
            SyslogServer = syslogServer;
            Token = token;

            Save();
        }

        private void Save()
        {
            Preferences.Set(SettingPrefix + nameof(RestEndpoint), RestEndpoint);
            Preferences.Set(SettingPrefix + nameof(Username), Username);
            Preferences.Set(SettingPrefix + nameof(Token), Token);
            Preferences.Set(SettingPrefix + nameof(SyslogServer), SyslogServer);
        }

        public void Reset()
        {
            Change(null, null, null, null);
            Preferences.Clear();
        }

        public string RestEndpoint { get; private set; }
        public string Username { get; private set; }
        public string Token { get; private set; }
        public string SyslogServer { get; private set; }
    }
}