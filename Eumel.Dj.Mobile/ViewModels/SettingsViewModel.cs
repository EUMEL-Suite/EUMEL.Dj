using Eumel.Dj.Mobile.Services;
using Xamarin.Forms;

namespace Eumel.Dj.Mobile.ViewModels
{
    public class SettingsViewModel : BaseViewModel
    {
        private readonly ISettingsService _settings;
        private readonly IPlayerService _player;
        private readonly IPlaylistService _playlist;
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
        public Command PauseCommand { get; set; }
        public Command RestartCommand { get; set; }
        public Command StopCommand { get; set; }
        public Command NextCommand { get; set; }

        public SettingsViewModel()
        {
            _settings = DependencyService.Get<ISettingsService>();
            _player = DependencyService.Get<IPlayerService>();
            _playlist = DependencyService.Get<IPlaylistService>();

            SyslogServer = _settings.SyslogServer;
            EumelServer = _settings.RestEndpoint;
            Username = _settings.Username;
            Token = _settings.Token;
            //UserIsAdmin = _settings.CheckUserIdAdmin().Result;
            UserIsAdmin = false;

            PlayCommand = new Command(async () => await _player.Play());
            PauseCommand = new Command(async () => await _player.Pause());
            RestartCommand = new Command(async () => await _player.Restart());
            StopCommand = new Command(async () => await _player.Stop());
            NextCommand = new Command(async () => await _player.Next());

            ClearSettingsCommand = new Command(async () =>
            {
                _settings.Reset();
                await _playlist.ClearMyVotes();
                await Shell.Current.GoToAsync("//Login");
            });
        }
        public async void OnAppearing()
        {
            IsBusy = true;

            UserIsAdmin = await _settings.CheckUserIsAdmin();
        }
    }
}