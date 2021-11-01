using System;
using System.Collections.Generic;
using System.Text;

namespace Eumel.Dj.Mobile.Models
{
    public class EumelDjMobileException : Exception
    {
        public EumelDjMobileException(string message, Exception innerException) : base(message, innerException) { }
    }
}
