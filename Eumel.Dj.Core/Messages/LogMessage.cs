using Eumel.Dj.WebServer.Exceptions;
using Microsoft.Extensions.Logging;

namespace Eumel.Dj.WebServer.Messages
{
    public class LogMessage : MessageRequest
    {
        public string Message { get; }
        public LogLevel Level { get; }
        public EumelDjException Exception { get; }

        public LogMessage(object sender, string message, LogLevel level, EumelDjException exception = null) : base(sender)
        {
            Message = message;
            Level = level;
            Exception = exception;
        }
    }
}