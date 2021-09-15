namespace Eumel.Dj.WebServer.Exceptions
{
    public class UnauthorizedEumelException : EumelDjException
    {
        public UnauthorizedEumelException(string message) : base(message)
        {
        }
    }
}