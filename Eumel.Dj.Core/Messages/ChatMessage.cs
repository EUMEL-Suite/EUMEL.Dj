namespace Eumel.Dj.Core.Messages
{
    public class ChatSendingMessage : MessageRequest
    {
        public ChatSendingMessage(object sender, string username, string message) : base(sender)
        {
            Username = username;
            Message = message;
        }

        public string Username { get; }
        public string Message { get; }
    }
    public class ChatReceivedMessage : MessageRequest
    {
        public ChatReceivedMessage(object sender, string username, string message) : base(sender)
        {
            Username = username;
            Message = message;
        }

        public string Username { get; }
        public string Message { get; }
    }
}