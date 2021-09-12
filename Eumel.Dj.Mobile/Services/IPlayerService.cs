using System.Threading.Tasks;

namespace Eumel.Dj.Mobile.Services
{
    public interface IPlayerService
    {
        Task Pause();
        Task Play();
        Task Stop();
        Task Next();
        Task Restart();
    }
}