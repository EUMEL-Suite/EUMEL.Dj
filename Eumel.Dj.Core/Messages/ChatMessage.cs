using Eumel.Dj.WebServer.Messages;

namespace Eumel.Dj.WebServer.Hubs
{
    public class ChatMessage : MessageRequest
    {
        public ChatMessage(object sender, string username, string message) : base(sender)
        {
            Username = username;
            Message = message;
        }

        public string Username { get; }
        public string Message { get; }
    }
}