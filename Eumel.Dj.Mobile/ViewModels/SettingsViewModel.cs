using Eumel.Dj.Mobile.Services;
using Xamarin.Forms;

namespace Eumel.Dj.Mobile.ViewModels
{
    public class SettingsViewModel : BaseViewModel
    {
        private readonly ISettingsService _settings;
        private string _eumelServer;
        private string _syslogServer;
        private string _username
            ;

        public string EumelServer
        {
            get => _eumelServer;
            set => SetProperty(ref _eumelServer, value);
        }

        public string SyslogServer
        {
            get => _syslogServer;
            set => SetProperty(ref _syslogServer, value);
        }

        public string Username
        {
            get => _username;
            set => SetProperty(ref _username, value);
        }

        public SettingsViewModel()
        {
            _settings = DependencyService.Get<ISettingsService>();

            SyslogServer = _settings.SyslogServer;
            EumelServer = _settings.RestEndpoint;
            Username = _settings.Username;
        }
        public void OnAppearing()
        {
            IsBusy = true;
        }
    }
}