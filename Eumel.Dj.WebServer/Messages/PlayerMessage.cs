namespace Eumel.Dj.WebServer.Messages
{
    public class PlayerMessage : MessageRequest<bool>
    {
        public PlayerControl PlayerAction { get; }
        public string SongId { get; }

        public PlayerMessage(object sender, PlayerControl playerAction, string songId = null)
            : base(sender)
        {
            PlayerAction = playerAction;
            SongId = songId;
        }

        public enum PlayerControl
        {
            Play,
            Pause,
            Continue,
            Stop
        }
    }
}