namespace Eumel.Dj.Core.Messages
{
    public class PlayerMessage : MessageRequest<bool>
    {
        public enum PlayerControl
        {
            Play,
            Pause,
            Stop,
            Next,
            Restart
        }

        public PlayerMessage(object sender, PlayerControl playerAction)
            : base(sender)
        {
            PlayerAction = playerAction;
        }

        public PlayerControl PlayerAction { get; }
    }
}