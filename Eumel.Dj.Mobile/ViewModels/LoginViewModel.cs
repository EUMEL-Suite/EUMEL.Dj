using System;
using System.Globalization;
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
        private readonly ISettingsService _settings;
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
            _settings = DependencyService.Get<ISettingsService>();
            Username = _settings.Username ?? Marvel.Names.Random();

            ScanResultCommand = new Command(ScanResult);
            RandomizeUsernameCommand = new Command(() => Username = Marvel.Names.Random());
        }

        private void ScanResult(object scan)
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

            Username = _settings.Username;
            Shell.Current.GoToAsync($"//{nameof(PlaylistPage)}");
        }

        private async Task<bool> GetSettingsForServer(string server)
        {
            var client = new EumelRestServiceFactory(server).Build();
            var token = await client.RequestSettingsAndTokenAsync(Username);

            _settings.Change(server, token.Username, token.SyslogServer, token.Usertoken);
            return true;
        }
    }
}
