namespace Eumel.Dj.WebServer.Hubs
{
    public class ChatHubMessage
    {
        public string Username { get; }
        public string Message { get; }

        public ChatHubMessage(string username, string message)
        {
            Username = username;
            Message = message;
        }
    }
}