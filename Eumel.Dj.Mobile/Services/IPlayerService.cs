using System.Threading.Tasks;

namespace Eumel.Dj.Mobile.Services
{
    public interface IPlayerService
    {
        Task Play();

        bool CanPlay { get; }
    }
}