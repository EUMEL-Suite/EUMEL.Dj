using System.Collections.Generic;

namespace Eumel.Dj.WebServer.Models
{
    public class DjPlaylist
    {
        public DjPlaylist(IEnumerable<VotedSong> votedSongs, IEnumerable<Song> unvotedNext)
        {
            VotedSongs = votedSongs;
            UnvotedNext = unvotedNext;
        }

        public IEnumerable<VotedSong> VotedSongs { get; }
        public IEnumerable<Song> UnvotedNext { get; }
        public Song CurrentSong { get; set; }
        public IEnumerable<Song> PastSongs { get; set; }
    }
}