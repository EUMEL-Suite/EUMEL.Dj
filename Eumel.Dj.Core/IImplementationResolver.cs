namespace Eumel.Dj.Core
{
    public interface IImplementationResolver<out T>
    {
        T Resolve();
    }
}