using System.Xml.Schema;
using Eumel.Dj.WebServer.Models;

namespace Eumel.Dj.WebServer.Messages
{
    public class VoteMessage : MessageRequest
    {
        public enum UpDownVote { Up, Down }

        public UpDownVote Direction { get; }
        public Song Song { get; }
        public string VotersName { get; }

        public VoteMessage(object sender, UpDownVote direction, Song song, string votersName) : base(sender)
        {
            Direction = direction;
            Song = song;
            VotersName = votersName;
        }
    }
}