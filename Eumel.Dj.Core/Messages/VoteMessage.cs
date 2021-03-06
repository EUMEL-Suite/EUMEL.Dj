using Eumel.Dj.Core.Models;

namespace Eumel.Dj.Core.Messages
{
    public class VoteMessage : MessageRequest
    {
        public enum UpDownVote
        {
            Up,
            Down
        }

        public VoteMessage(object sender, UpDownVote direction, Song song, string votersName) : base(sender)
        {
            Direction = direction;
            Song = song;
            VotersName = votersName;
        }

        public UpDownVote Direction { get; }
        public Song Song { get; }
        public string VotersName { get; }
    }
}