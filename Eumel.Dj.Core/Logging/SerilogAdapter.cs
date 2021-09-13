using System;
using Eumel.Dj.WebServer.Exceptions;
using Serilog;
using Serilog.Context;

namespace Eumel.Dj.WebServer.Logging
{
    public class SerilogAdapter : IEumelLogger
    {
        private readonly ILogger _serilogLogger;

        public SerilogAdapter(ILogger serilogLogger)
        {
            _serilogLogger = serilogLogger ?? throw new ArgumentNullException(nameof(serilogLogger));
        }

        public void Verbose(string message)
        {
            _serilogLogger.Verbose(message);
        }

        public void Debug(string message)
        {
            _serilogLogger.Debug(message);
        }

        public void Information(string message, params object[] propertyValues)
        {
            _serilogLogger.Information(message, propertyValues);
        }

        public void Information(string message)
        {
            _serilogLogger.Information(message);
        }

        public void Warning(string message)
        {
            _serilogLogger.Warning(message);
        }

        public void Error(string message, Exception ex)
        {
            _serilogLogger.Error(ex, message);
        }

        [Obsolete("this method is not longer supported, please use Method with (message, exception) ")]
        public void Error(string message)
        {
            _serilogLogger.Error(new EumelDjException(message), message);
        }

        [Obsolete("this method is not longer supported, please use Method with (message, exception) ")]
        public void Fatal(string message)
        {
            _serilogLogger.Fatal(new EumelDjException(message), message);
        }

        public void Fatal(string message, Exception ex)
        {
            _serilogLogger.Fatal(ex, message);
        }

        public void Fatal(string message, Exception ex, params object[] propertyValues)
        {
            _serilogLogger.Fatal(ex, message, propertyValues);
        }

        public IDisposable PushProperty(string name, object value)
        {
            return LogContext.PushProperty(name, value);
        }
    }
}