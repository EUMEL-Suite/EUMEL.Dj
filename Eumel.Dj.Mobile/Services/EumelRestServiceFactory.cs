using System.Net.Http;
using Xamarin.Forms;

namespace Eumel.Dj.Mobile.Services
{
    public class EumelRestServiceFactory : IEumelRestServiceFactory
    {
        private readonly ISettingsService _settingsService;

        public EumelRestServiceFactory()
        {
            _settingsService = DependencyService.Get<ISettingsService>();
        }

        public EumelDjServiceClient Build()
        {
            var cl = new HttpClientHandler();
            cl.ServerCertificateCustomValidationCallback = (message, certificate2, arg3, arg4) => true;

            var client = new HttpClient(cl);

            return new EumelDjServiceClient(_settingsService.RestEndpoint, client);
        }
    }
}