using System;

namespace Eumel.Dj.WebServer
{
    public class EumelDjException : Exception
    {
        public EumelDjException(string message) : base(message)
        {
        }
    }
}