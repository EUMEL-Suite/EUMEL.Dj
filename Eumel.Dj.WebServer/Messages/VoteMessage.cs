using Eumel.Dj.WebServer.Models;

namespace Eumel.Dj.WebServer.Messages
{
    public class VoteMessage : MessageRequest<bool>
    {
        public enum UpDownVote { Up, Down }

        public UpDownVote Direction { get; }
        public Song Song { get; }

        public VoteMessage(object sender, UpDownVote direction, Song song) : base(sender)
        {
            Direction = direction;
            Song = song;
        }
    }
}