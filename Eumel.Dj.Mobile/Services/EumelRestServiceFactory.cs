using System.Net.Http;
using Xamarin.Forms;

namespace Eumel.Dj.Mobile.Services
{
    public class EumelRestServiceFactory : IEumelRestServiceFactory
    {
        private readonly ISettingsService _settingsService;

        #region Private class to support "endpoint only" constructor

        private class EndpointOnlySettings : ISettingsService
        {
            public string RestEndpoint { get; set; }
            public string Username { get; }
            public string Token { get; }
            public string SyslogServer { get; }
            public void Change(string restEndpoint, string username, string syslogServer, string token) { }
            public void Reset() { }
        }

        #endregion

        public EumelRestServiceFactory()
        {
            _settingsService = DependencyService.Get<ISettingsService>();
        }

        public EumelRestServiceFactory(string endpoint)
        {
            var settings = new EndpointOnlySettings() { RestEndpoint = endpoint };
            _settingsService = settings;
        }

        public EumelDjServiceClient Build()
        {
            var cl = new HttpClientHandler();
            cl.ServerCertificateCustomValidationCallback = (message, certificate2, arg3, arg4) => true;

            var client = new HttpClient(cl);
            client.DefaultRequestHeaders.Add("usertoken", _settingsService.Token);

            return new EumelDjServiceClient(_settingsService.RestEndpoint, client);
        }
    }
}