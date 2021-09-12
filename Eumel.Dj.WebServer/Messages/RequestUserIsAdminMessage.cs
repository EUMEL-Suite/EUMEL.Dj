namespace Eumel.Dj.WebServer.Messages
{
    public class RequestUserIsAdminMessage : MessageRequest<bool>
    {
        public string Username { get; }

        public RequestUserIsAdminMessage(object sender, string username) : base(sender)
        {
            Username = username;
        }
    }
}