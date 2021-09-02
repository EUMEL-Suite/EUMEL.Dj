using Microsoft.Extensions.Logging;

namespace Eumel.Dj.WebServer.Messages
{
    public class LogMessage : MessageRequest
    {
        public string Message { get; }
        public LogLevel Level { get; }

        public LogMessage(object sender, string message, LogLevel level) : base(sender)
        {
            Message = message;
            Level = level;
        }
    }
}