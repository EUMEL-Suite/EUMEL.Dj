using System.Net.Sockets;

namespace Eumel.Dj.WebServer.Messages
{
    public class PlayerMessageRequest : MessageRequest<bool>
    {
        public PlayerControl PlayerAction { get; }
        public string Location { get; }

        public PlayerMessageRequest(object sender, PlayerControl playerAction, string location = null)
            : base(sender)
        {
            PlayerAction = playerAction;
            Location = location;
        }

        public enum PlayerControl
        {
            Play,
            Pause,
            Continue
        }
    }
}