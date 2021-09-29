using Eumel.Dj.Mobile.Views;
using Xamarin.Forms;

namespace Eumel.Dj.Mobile.ViewModels
{
    public class SettingsViewModel : BaseViewModel
    {
        private string _eumelServer;
        private string _syslogServer;
        private string _token;
        private bool _userIsAdmin;
        private string _username;
        private string _playlistSongCount;
        private string _playlistName;

        public SettingsViewModel()
        {
            SyslogServer = Settings.SyslogServer;
            EumelServer = Settings.RestEndpoint;
            Username = Settings.Username;
            Token = Settings.Token;
            UserIsAdmin = false;

            PlayCommand = new Command(() => TryOrRedirectToLoginAsync(() => PlayerService.Play(), "Player Play"));
            PauseCommand = new Command(() => TryOrRedirectToLoginAsync(() => PlayerService.Pause(), "Player Pause"));
            RestartCommand = new Command(() => TryOrRedirectToLoginAsync(() => PlayerService.Restart(), "Player Restart"));
            StopCommand = new Command(() => TryOrRedirectToLoginAsync(() => PlayerService.Stop(), "Player Stop"));
            NextCommand = new Command(() => TryOrRedirectToLoginAsync(() => PlayerService.Next(), "Player Next"));

            ClearSettingsCommand = new Command(async () =>
            {
                await Settings.Logout();
                Settings.Reset();
                Application.Current.MainPage = new LoginPage { BackgroundColor = Color.White };
            });
        }

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

        public string PlaylistName
        {
            get => _playlistName;
            set => SetProperty(ref _playlistName, value);
        }

        public string PlaylistSongCount
        {
            get => _playlistSongCount;
            set => SetProperty(ref _playlistSongCount, value);
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

        public async void OnAppearing()
        {
            IsBusy = true;

            UserIsAdmin = await Settings.CheckUserIsAdmin();

            var source = await SongService.GetSongsAsync(true);
            PlaylistName = source.Name;
            PlaylistSongCount = source.NumberOfSongs.ToString();
        }
    }
}