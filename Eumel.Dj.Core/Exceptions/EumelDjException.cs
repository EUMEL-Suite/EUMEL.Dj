using System;

namespace Eumel.Dj.Core.Exceptions
{
    public class EumelDjException : Exception
    {
        public EumelDjException(string message) : base(message)
        {
        }
    }
}