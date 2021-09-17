namespace Eumel.Dj.Core.Messages
{
    public class UserAddedMessage : MessageRequest
    {
        public string Username { get; }

        public UserAddedMessage(object sender, string username) : base(sender)
        {
            Username = username;
        }
    }
}