namespace Eumel.Dj.Core.Logging
{
    public interface ILoggerFactory
    {
        IEumelLogger Build(ILoggerSettings settings);
    }
}