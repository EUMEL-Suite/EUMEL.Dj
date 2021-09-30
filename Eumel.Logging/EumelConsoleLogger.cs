using System;
using Eumel.Core.Logging;

namespace Eumel.Logging
{
    public class EumelConsoleLogger : IEumelLogger
    {
        private readonly LoggerSettings _settings;

        public EumelConsoleLogger(LoggerSettings settings)
        {
            _settings = settings ?? throw new ArgumentNullException(nameof(settings));
        }

        public void Verbose(string message)
        {
            if (_settings.UseConsole)
                Console.WriteLine($"Verbose: {message}");
            if (_settings.UseDebug)
                System.Diagnostics.Debug.WriteLine($"Verbose: {message}");
        }

        public void Debug(string message)
        {
            if (_settings.UseConsole)
                Console.WriteLine($"Debug: {message}");
            if (_settings.UseDebug)
                System.Diagnostics.Debug.WriteLine($"Debug: {message}");
        }

        public void Information(string message)
        {
            if (_settings.UseConsole)
                Console.WriteLine($"Information: {message}");
            if (_settings.UseDebug)
                System.Diagnostics.Debug.WriteLine($"Information: {message}");
        }

        public void Warning(string message)
        {
            if (_settings.UseConsole)
                Console.WriteLine($"Warning: {message}");
            if (_settings.UseDebug)
                System.Diagnostics.Debug.WriteLine($"Warning: {message}");
        }

        public void Error(string message, Exception ex)
        {
            if (_settings.UseConsole)
                Console.WriteLine($"Error: {message} [{ex.Message}]");
            if (_settings.UseDebug)
                System.Diagnostics.Debug.WriteLine($"Error: {message} [{ex.Message}]");
        }

        public void Fatal(string message, Exception ex)
        {
            if (_settings.UseConsole)
                Console.WriteLine($"Fatal: {message} [{ex.Message}]");
            if (_settings.UseDebug)
                System.Diagnostics.Debug.WriteLine($"Fatal: {message} [{ex.Message}]");
        }

        public IDisposable PushProperty(string name, object value)
        {
            return null;
        }
    }
}