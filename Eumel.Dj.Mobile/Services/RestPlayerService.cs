using System.Threading.Tasks;
using Xamarin.Forms;

namespace Eumel.Dj.Mobile.Services
{
    public class RestPlayerService : IPlayerService
    {
        private readonly EumelDjServiceClient _service;

        public RestPlayerService()
        {
            _service = DependencyService.Get<IEumelRestServiceFactory>().Build();
        }

        public async Task Pause()
        {
            await _service.PauseAsync();
        }

        public async Task Play()
        {
            await _service.PlayAsync();
        }

        public async Task Stop()
        {
            await _service.StopAsync();
        }

        public async Task Next()
        {
            await _service.NextAsync();
        }

        public async Task Restart()
        {
            await _service.RestartAsync();
        }
    }
}