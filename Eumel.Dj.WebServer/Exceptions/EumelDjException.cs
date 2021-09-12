using System;

namespace Eumel.Dj.WebServer.Exceptions
{
    public class EumelDjException : Exception
    {
        public EumelDjException(string message) : base(message)
        {
        }
    }
}