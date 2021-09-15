namespace Eumel.Dj.WebServer.Exceptions
{
    public class InvalidTokenException : EumelDjException
    {
        public InvalidTokenException(string message) : base(message)
        {
        }
    }
}