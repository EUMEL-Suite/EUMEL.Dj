namespace Eumel.Dj.Core.Messages
{
    public class PlayerStatusMessage : MessageRequest<PlayerStatus>
    {
        public PlayerStatusMessage(object sender) : base(sender) { }
    }
}