using System;

namespace Eumel.Dj.Core.Logging
{
    public class EumelConsoleLogger : IEumelLogger
    {
        public void Verbose(string message)
        {
            Console.WriteLine($"Verbose: {message}");
        }

        public void Debug(string message)
        {
            Console.WriteLine($"Debug: {message}");
        }

        public void Information(string message)
        {
            Console.WriteLine($"Information: {message}");
        }

        public void Warning(string message)
        {
            Console.WriteLine($"Warning: {message}");
        }

        public void Error(string message, Exception ex)
        {
            Console.WriteLine($"Error: {message} [{ex.Message}]");
        }

        public void Fatal(string message, Exception ex)
        {
            Console.WriteLine($"Fatal: {message} [{ex.Message}]");
        }

        public IDisposable PushProperty(string name, object value)
        {
            return null;
        }
    }
}