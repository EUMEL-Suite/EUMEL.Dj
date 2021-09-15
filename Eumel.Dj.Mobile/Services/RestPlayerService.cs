using System.Threading.Tasks;
using Xamarin.Forms;

namespace Eumel.Dj.Mobile.Services
{
    public class RestPlayerService : IPlayerService
    {
        private EumelDjServiceClient Service => DependencyService.Get<IEumelRestServiceFactory>().Build();

        public async Task Pause()
        {
            await Service.PauseAsync();
        }

        public async Task Play()
        {
            await Service.PlayAsync();
        }

        public async Task Stop()
        {
            await Service.StopAsync();
        }

        public async Task Next()
        {
            await Service.NextAsync();
        }

        public async Task Restart()
        {
            await Service.RestartAsync();
        }
    }
}