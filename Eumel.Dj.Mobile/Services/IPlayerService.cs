using System.Threading.Tasks;

namespace Eumel.Dj.Mobile.Services
{
    public interface IPlayerService
    {
        Task Continue();
        bool CanContinue { get; }
        bool CanPlay { get; }
        bool CanStop { get; }
        Task Play();
        Task Stop();
    }
}