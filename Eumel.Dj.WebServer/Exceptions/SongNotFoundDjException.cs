namespace Eumel.Dj.WebServer
{
    public class SongNotFoundDjException : EumelDjException
    {
        public SongNotFoundDjException(string message) : base(message)
        {
        }
    }
}