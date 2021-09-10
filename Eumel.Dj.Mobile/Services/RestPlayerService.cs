using System.Threading.Tasks;
using Xamarin.Forms;

namespace Eumel.Dj.Mobile.Services
{
    public class RestPlayerService : IPlayerService
    {
        private readonly EumelDjServiceClient _service;
        private readonly ISettingsService _settings;

        public RestPlayerService()
        {
            _service = DependencyService.Get<IEumelRestServiceFactory>().Build();
            _settings = DependencyService.Get<ISettingsService>();
        }
        public async Task Play()
        {
            await _service.ContinueAsync();
        }

        public bool CanPlay
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_settings.Token))
                    return false;

                var status = _service.StatusAsync().Result;
                return status != PlayerControl.Play;
            }
        }
    }
}