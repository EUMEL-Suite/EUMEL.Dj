using Eumel.Dj.Mobile.Services;
using Xamarin.Forms;

namespace Eumel.Dj.Mobile.ViewModels
{
    public class SettingsViewModel : BaseViewModel
    {
        private readonly ISettingsService _settings;
        private readonly IPlaylistService _playlist;
        private string _eumelServer;
        private string _syslogServer;
        private string _username ;
        private string _token;

        public Command ClearSettingsCommand { get; }

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

        public string Token
        {
            get => _token;
            set => SetProperty(ref _token, value);
        }

        public SettingsViewModel()
        {
            _settings = DependencyService.Get<ISettingsService>();
            _playlist = DependencyService.Get<IPlaylistService>();

            SyslogServer = _settings.SyslogServer;
            EumelServer = _settings.RestEndpoint;
            Username = _settings.Username;
            Token = _settings.Token;

            ClearSettingsCommand = new Command(() =>
            {
                _settings.Reset();
                // TODO IMPLEMENT _playlist.ClearMyVotes();
                Shell.Current.GoToAsync("//Login");
            });
        }
        public void OnAppearing()
        {
            IsBusy = true;
        }
    }
}