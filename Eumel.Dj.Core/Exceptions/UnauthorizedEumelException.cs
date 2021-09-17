using System.Diagnostics;

namespace Eumel.Dj.Core.Exceptions
{
    public class UnauthorizedEumelException : EumelDjException
    {
        public UnauthorizedEumelException(string message) : base(message)
        {
            // this is to make sure, the constant for the hub message match the method names
            Debug.Assert(nameof(InvalidTokenException) == Constants.UnauthorizedEumelException, message: "Class name must be added with the same name to constants");
        }
    }
}