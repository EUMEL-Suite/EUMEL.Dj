using System.Diagnostics;

namespace Eumel.Dj.WebServer.Exceptions
{
    public class InvalidTokenException : EumelDjException
    {
        public InvalidTokenException(string message) : base(message)
        {
            // this is to make sure, the constant for the hub message match the method names
            Debug.Assert(nameof(InvalidTokenException) == Constants.InvalidTokenException, message: "Class name must be added with the same name to constants");
        }
    }
}