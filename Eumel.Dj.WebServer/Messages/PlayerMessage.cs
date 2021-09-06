namespace Eumel.Dj.WebServer.Messages
{
    public class PlayerMessage : MessageRequest<bool>
    {
        public enum PlayerControl
        {
            Play,
            Pause,
            Continue,
            Stop
        }

        public PlayerMessage(object sender, PlayerControl playerAction, string songId = null)
            : base(sender)
        {
            PlayerAction = playerAction;
            SongId = songId;
        }

        public PlayerControl PlayerAction { get; }
        public string SongId { get; }
    }
}