using System.Collections.Generic;
using Newtonsoft.Json;

namespace Eumel.Dj.WebServer.Models
{
    public class DjPlaylist
    {
        public DjPlaylist(IEnumerable<VotedSong> upcomingSongs)
        {
            UpcomingSongs = upcomingSongs;
        }

        public IEnumerable<VotedSong> UpcomingSongs { get; }
        [JsonProperty(Required = Required.AllowNull)]
        public VotedSong CurrentSong { get; set; }
        public IEnumerable<VotedSong> PastSongs { get; set; }
    }
}