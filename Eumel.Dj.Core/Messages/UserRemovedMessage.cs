namespace Eumel.Dj.Core.Messages
{
    public class UserRemovedMessage : MessageRequest
    {
        public string Username { get; }

        public UserRemovedMessage(object sender, string username) : base(sender)
        {
            Username = username;
        }
    }
}