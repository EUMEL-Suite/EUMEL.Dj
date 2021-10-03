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
            SyslogServer = Settings.SyslogServer ?? "not supported";
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

        public string PrivacyStatement
        {
            get { return @"# Privacy Policy
Your privacy is important to us.IT is EUMEL SUITE's policy to respect your privacy and comply with any applicable law and regulation regarding any personal information we may collect about you, including via our app, EUMEL Dj and its associated services. Personal information is any information about you which can be used to identify you. This includes information about you as a person (such as name, address, and date of birth), your devices, payment details, and even information about how you use an app or online service.

The EUMEL Dj app uses third-party sites and services, so be aware that those sites and services have their own privacy policies.All information as votes, chat messages, your nickname and -if configured by the service - debug information is sent to the third party services. By sending information to the third-party service, you should read their posted privacy policy information about how they collect and use personal information. This Privacy Policy does not apply to any of your activities after you leave our app.

This policy is effective as of 3 October 2021.

Last updated: 3 October 2021

# Information We Collect

The app stores configuration data to access the third party service until the user uses the log out functionality of the app. 

# Log Data

This app does not log any information internally or external service.

# Device Data

This app does not access any device data or sends any device data to a third party service.

# Personal Information

This app uses a user - definable nickname to identify the user at the backend service.The user does not need to reveal his real name.

# Sensitive Information

No sensitive information is stored except of the nickname, the user selected during login.

# User-Generated Content

The following information is send to the third party service and share with other users, using the same third party service through the app or any other user interface for this service.

* nickname as anonymised username
* chat messages(sent as nickname)
* votes(sent as nickname)

The app supports a social chat, which can be shared with other users, who use the same third part service.Votes for songs are sent to the third party service and can be stored, according to the orivacy policy of the

# Your Rights and Controlling Your Personal Information

A logout sends a request to the third party service to clear user information.The information is removed according to the privacy policy of third party service.

# Contact Us

Thomas Ley
Rosseler Str. 29   
51570 Windeck

eumel-suite @outlook.de"; }
        }

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