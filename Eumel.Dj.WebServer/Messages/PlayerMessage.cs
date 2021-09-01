﻿namespace Eumel.Dj.WebServer.Messages
{
    public class PlayerMessage : MessageRequest<bool>
    {
        public PlayerControl PlayerAction { get; }
        public string Location { get; }

        public PlayerMessage(object sender, PlayerControl playerAction, string location = null)
            : base(sender)
        {
            PlayerAction = playerAction;
            Location = location;
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