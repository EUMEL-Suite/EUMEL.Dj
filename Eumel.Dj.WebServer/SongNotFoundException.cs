using System;

namespace Eumel.Dj.WebServer
{
    public class SongNotFoundDjException : EumelDjException
    {
        public SongNotFoundDjException(string message) : base(message) { }
    }

    public class EumelDjException : Exception
    {
        public EumelDjException(string message) : base(message) { }
    }
}