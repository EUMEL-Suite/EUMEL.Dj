using Xamarin.Forms;

namespace Eumel.Dj.Mobile.ViewModels
{
    public class SettingsViewModel : BaseViewModel
    {
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
            SyslogServer = Settings.SyslogServer;
            EumelServer = Settings.RestEndpoint;
            Username = Settings.Username;
            Token = Settings.Token;
            //UserIsAdmin = Settings.CheckUserIdAdmin().Result;
            UserIsAdmin = false;

            PlayCommand = new Command(async () => await PlayerService.Play());
            PauseCommand = new Command(async () => await PlayerService.Pause());
            RestartCommand = new Command(async () => await PlayerService.Restart());
            StopCommand = new Command(async () => await PlayerService.Stop());
            NextCommand = new Command(async () => await PlayerService.Next());

            ClearSettingsCommand = new Command(async () =>
            {
                Settings.Reset();
                await PlaylistService.ClearMyVotes();
                await Shell.Current.GoToAsync("//Login");
            });
        }
        public async void OnAppearing()
        {
            IsBusy = true;

            UserIsAdmin = await Settings.CheckUserIsAdmin();
        }
    }
}