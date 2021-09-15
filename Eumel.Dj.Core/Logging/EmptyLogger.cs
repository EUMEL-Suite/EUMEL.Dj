﻿using System;

namespace Eumel.Dj.WebServer.Logging
{
    public class EmptyLogger : IEumelLogger
    {
        public void Verbose(string message) { }

        public void Debug(string message) { }

        public void Information(string message, params object[] propertyValues) { }

        public void Information(string message) { }

        public void Warning(string message) { }

        public void Error(string message) { }

        public void Error(string message, Exception ex) { }

        public void Fatal(string message) { }

        public void Fatal(string message, Exception ex) { }

        public void Fatal(string message, Exception ex, params object[] propertyValues) { }

        public IDisposable PushProperty(string name, object value) { return null; }
    }
}