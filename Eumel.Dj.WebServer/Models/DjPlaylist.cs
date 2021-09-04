using System.Collections.Generic;

namespace Eumel.Dj.WebServer.Models
{
    public class DjPlaylist
    {
        public DjPlaylist(IEnumerable<VotedSong> upcomingSongs)
        {
            UpcomingSongs = upcomingSongs;
        }

        public IEnumerable<VotedSong> UpcomingSongs { get; }
        public VotedSong CurrentSong { get; set; }
        public IEnumerable<VotedSong> PastSongs { get; set; }
    }
}