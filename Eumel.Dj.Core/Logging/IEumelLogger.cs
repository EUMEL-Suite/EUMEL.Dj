using System;

namespace Eumel.Dj.Core.Logging
{
    public interface IEumelLogger
    {
        void Verbose(string message);

        void Debug(string message);

        void Information(string message);

        void Warning(string message);

        void Error(string message, Exception ex);


        void Fatal(string message, Exception ex);

        IDisposable PushProperty(string name, object value);
    }
}