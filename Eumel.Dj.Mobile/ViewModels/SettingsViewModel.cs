using Eumel.Dj.Mobile.Services;
using Xamarin.Forms;

namespace Eumel.Dj.Mobile.ViewModels
{
    public class SettingsViewModel : BaseViewModel
    {
        private readonly ISettingsService _settings;
        private readonly IPlaylistService _playlist;
        private readonly IPlayerService _player;
        private string _eumelServer;
        private string _syslogServer;
        private string _username ;
        private string _token;
        private bool _userIsAdmin;

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

        public bool UserIsAdmin
        {
            get => _userIsAdmin;
            set => SetProperty(ref _userIsAdmin, value);
        }

        public Command PlayCommand { get; set; }
        public Command StopCommand { get; set; }
        public Command ContinueCommand { get; set; }

        public SettingsViewModel()
        {
            _settings = DependencyService.Get<ISettingsService>();
            _playlist = DependencyService.Get<IPlaylistService>();
            _player = DependencyService.Get<IPlayerService>();

            SyslogServer = _settings.SyslogServer;
            EumelServer = _settings.RestEndpoint;
            Username = _settings.Username;
            Token = _settings.Token;
            UserIsAdmin = Username.Contains("Admin");

            ContinueCommand = new Command(async () => await _player.Continue());
            StopCommand = new Command(async () => await _player.Stop());
            PlayCommand = new Command(async () => await _player.Play());

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

            SyslogServer = _settings.SyslogServer;
            EumelServer = _settings.RestEndpoint;
            Username = _settings.Username;
            Token = _settings.Token;
            UserIsAdmin = Username.Contains("Admin");
        }
    }
}