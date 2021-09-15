using System.Threading.Tasks;
using Eumel.Dj.Mobile.Data;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Eumel.Dj.Mobile.Services
{
    public class SettingsService : ISettingsService
    {
        private EumelDjServiceClient Service => DependencyService.Get<IEumelRestServiceFactory>().Build();

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
            DependencyService.Get<ISyslogService>().Debug("Settings changed");
            RestEndpoint = restEndpoint;
            Username = username;
            SyslogServer = syslogServer;
            Token = token;

            Save();
        }

        private void Save()
        {
            DependencyService.Get<ISyslogService>().Debug("Settings saved");
            Preferences.Set(SettingPrefix + nameof(RestEndpoint), RestEndpoint);
            Preferences.Set(SettingPrefix + nameof(Username), Username);
            Preferences.Set(SettingPrefix + nameof(Token), Token);
            Preferences.Set(SettingPrefix + nameof(SyslogServer), SyslogServer);
        }

        public void Reset()
        {
            DependencyService.Get<ISyslogService>().Debug("Settings cleared");
            Change(null, null, null, null);
            Preferences.Clear();
        }

        public async Task<bool> CheckUserIsAdmin()
        {
            if (string.IsNullOrWhiteSpace(Token)) return false;

            return await Service.CheckUserIsAdminAsync();
        }

        public async Task<bool> TokenIsInvalid()
        {
            var isValid = await Service.TokenIsValidAsync();
            return !isValid;
        }

        public async Task Logout()
        {
            await Service.LogoutAsync();
        }

        public string RestEndpoint { get; private set; }
        public string Username { get; private set; }
        public string Token { get; private set; }
        public string SyslogServer { get; private set; }
    }
}