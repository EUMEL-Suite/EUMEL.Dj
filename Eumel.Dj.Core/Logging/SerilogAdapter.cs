using System;
using Eumel.Dj.Core.Exceptions;
using Serilog;
using Serilog.Context;

namespace Eumel.Dj.Core.Logging
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

        public void Fatal(string message, Exception ex)
        {
            _serilogLogger.Fatal(ex, message);
        }

        public IDisposable PushProperty(string name, object value)
        {
            return LogContext.PushProperty(name, value);
        }
    }
}