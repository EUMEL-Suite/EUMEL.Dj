namespace Eumel.Dj.WebServer.Messages
{
    public class ClearMyVotesMessage : MessageRequest
    {
        public string Username { get; }

        public ClearMyVotesMessage(object sender, string username) : base(sender)
        {
            Username = username;
        }
    }
}