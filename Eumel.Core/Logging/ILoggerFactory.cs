namespace Eumel.Core.Logging
{
    public interface ILoggerFactory
    {
        IEumelLogger Build(LoggerSettings settings);
    }
}