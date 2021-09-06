namespace Eumel.Dj.WebServer.Logging
{
    public interface ILoggerFactory
    {
        IEumelLogger Build(ILoggerSettings settings);
    }
}