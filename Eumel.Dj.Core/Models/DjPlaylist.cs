using System.Collections.Generic;
using Newtonsoft.Json;

namespace Eumel.Dj.Core.Models
{
    public class DjPlaylist
    {
        public DjPlaylist(IEnumerable<VotedSong> upcomingSongs)
        {
            UpcomingSongs = upcomingSongs;
        }

        public IEnumerable<VotedSong> UpcomingSongs { get; }
        // todo checkme 
        [JsonProperty(Required = Required.AllowNull)]
        public VotedSong CurrentSong { get; set; }
        public IEnumerable<VotedSong> PastSongs { get; set; }
    }
}