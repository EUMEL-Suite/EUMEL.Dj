using System.Collections.Generic;

namespace Eumel.Dj.WebServer.Models
{
    public class VotedSong : Song
    {
        public IList<string> Voters { get; set; }
    }
}