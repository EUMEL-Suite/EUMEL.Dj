using System.Threading.Tasks;
using SyslogLogging;
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
            _service =DependencyService.Get<IEumelRestServiceFactory>().Build();
        }

        public async Task<DjPlaylist> Get()
        {
            _log.Debug("Getting playlist");
            return await _service.GetPlaylistAsync();
        }
    }

    public interface ISyslogService
    {
        void Debug(string msg);
    }

    public class SyslogService : ISyslogService
    {
        private readonly LoggingModule _log;

        public SyslogService()
        {
            _log = new LoggingModule("192.168.178.37", 514, true);
            _log.Debug("This is a debug message!");
        }

        public void Debug(string msg)
        {
            _log.Debug(msg);
        }
    }
}