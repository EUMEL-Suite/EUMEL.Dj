using System.Threading.Tasks;
using Xamarin.Forms;

namespace Eumel.Dj.Mobile.Services
{
    public class RestPlaylistService : IPlaylistService
    {
        private readonly ISyslogService _log;
        private readonly EumelDjServiceClient _service;

        public RestPlaylistService()
        {
            _log = DependencyService.Get<ISyslogService>();
            _service = DependencyService.Get<IEumelRestServiceFactory>().Build();
        }

        public async Task<DjPlaylist> Get()
        {
            _log.Debug("Getting playlist");
            return await _service.GetPlaylistAsync();
        }
    }
}