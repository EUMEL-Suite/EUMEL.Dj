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

        public void Information(string message, params object[] propertyValues)
        {
            // TODO a method should have an implementation
        }

        public void Information(string message)
        {
            // TODO a method should have an implementation
        }

        public void Warning(string message)
        {
            // TODO a method should have an implementation
        }

        public void Error(string message)
        {
            Console.WriteLine($"Error: {message}");
        }

        public void Error(string message, Exception ex)
        {
            Console.WriteLine($"Error: {message} [{ex.Message}]");
        }

        public void Fatal(string message)
        {
            // TODO a method should have an implementation
        }

        public void Fatal(string message, Exception ex)
        {
            // TODO a method should have an implementation
        }

        public void Fatal(string message, Exception ex, params object[] propertyValues)
        {
            // TODO a method should have an implementation
        }

        public IDisposable PushProperty(string name, object value)
        {
            return null;
        }
    }
}