using System;

namespace Eumel.Dj.WebServer.Logging
{
    public interface IEumelLogger
    {
        void Verbose(string message);

        void Debug(string message);

        void Information(string message, params object[] propertyValues);

        void Information(string message);

        void Warning(string message);

        [Obsolete("this method is not longer supported, please use Method with(message, exception)")]
        void Error(string message);

        void Error(string message, Exception ex);

        [Obsolete("this method is not longer supported, please use Method with(message, exception)")]
        void Fatal(string message);

        void Fatal(string message, Exception ex);

        void Fatal(string message, Exception ex, params object[] propertyValues);

        IDisposable PushProperty(string name, object value);
    }
}