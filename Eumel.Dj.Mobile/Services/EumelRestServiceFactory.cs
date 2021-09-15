using System;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace Eumel.Dj.Mobile.Services
{
    public class EumelRestServiceFactory : IEumelRestServiceFactory
    {
        // we need to define a func or factory because the method how to get the setting change depending on constructor
        private Func<ISettingsService> SettingsFactory { get; }

        #region Private class to support "endpoint only" constructor

        private class EndpointOnlySettings : ISettingsService
        {
            public string RestEndpoint { get; set; }
            public string Username { get; }
            public string Token { get; }
            public string SyslogServer { get; }
            public void Change(string restEndpoint, string username, string syslogServer, string token) { }
            public void Reset() { }

            public Task<bool> CheckUserIsAdmin() { return Task.FromResult(false); }
        }

        #endregion

        public EumelRestServiceFactory()
        {
            SettingsFactory = () => DependencyService.Get<ISettingsService>();
        }

        public EumelRestServiceFactory(string endpoint)
        {
            var settings = new EndpointOnlySettings() { RestEndpoint = endpoint };
            SettingsFactory = () => settings;
        }

        public EumelDjServiceClient Build()
        {
            var settings = SettingsFactory();
            var cl = new HttpClientHandler();
            cl.ServerCertificateCustomValidationCallback = (message, certificate2, arg3, arg4) => true;

            // the client needs to send a token to identify himself
            var client = new HttpClient(cl);
            client.DefaultRequestHeaders.Add(Constants.UserToken, settings.Token);

            return new EumelDjServiceClient(settings.RestEndpoint, client);
        }
    }
}