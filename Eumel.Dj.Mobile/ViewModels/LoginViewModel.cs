using System.Threading.Tasks;
using Eumel.Dj.Mobile.Data;
using Eumel.Dj.Mobile.Services;
using Eumel.Dj.Mobile.Views;
using Xamarin.Forms;
using ZXing;

namespace Eumel.Dj.Mobile.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private string _username = string.Empty;
        private string _userHint;

        public string Username
        {
            get => _username;
            set => SetProperty(ref _username, value);
        }

        public string UserHint
        {
            get => _userHint;
            set => SetProperty(ref _userHint, value);
        }

        public Command ScanResultCommand { get; }

        public Command RandomizeUsernameCommand { get; }

        public LoginViewModel()
        {
            Username = Settings.Username ?? Marvel.Names.Random();

            ScanResultCommand = new Command(ScanResult);
            RandomizeUsernameCommand = new Command(() => Username = Marvel.Names.Random());
        }

        private async void ScanResult(object scan)
        {
            // dont care what you scan, username must be filled
            if (string.IsNullOrWhiteSpace(Username))
            {
                UserHint = "Username must be set in order to login";
                return;
            }

            // this must be a scan result to work
            if (!(scan is Result scanResult))
            {
                UserHint = "Something went wrong during scanning";
                return;
            }

            // and if it not a QR Code, it is not an eumel code
            if (scanResult.BarcodeFormat != BarcodeFormat.QR_CODE)
            {
                UserHint = "Please scan an EUMEL DJ QR Code";
                return;
            }

            var server = scanResult.Text;
            // todo validate that this is a proper url

            if (!GetSettingsForServer(server).Result)
            {
                UserHint = "Cannot get settings from server";
                return;
            }

            Username = Settings.Username;





            // RECREATE SERVICE OR USER NEW TOKEN! THE OLD TOKEN IS NOT ADDED IN HEADER







            Application.Current.Dispatcher.BeginInvokeOnMainThread(() =>
            {
                if (Shell.Current == null)
                    Application.Current.MainPage = new AppShell();

                // ReSharper disable once PossibleNullReferenceException
                Shell.Current.GoToAsync(nameof(PlaylistPage));
            });
        }

        private async Task<bool> GetSettingsForServer(string server)
        {
            var client = new EumelRestServiceFactory(server).Build();
            var token = await client.RequestSettingsAndTokenAsync(Username);

            Settings.Change(server, token.Username, token.SyslogServer, token.Usertoken);
            return true;
        }


        public void OnAppearing()
        {
        }
    }
}
