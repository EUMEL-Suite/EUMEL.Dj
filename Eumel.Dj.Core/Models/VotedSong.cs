using System.Collections.Generic;

namespace Eumel.Dj.Core.Models
{
    public class VotedSong : Song
    {
        public IList<string> Voters { get; set; }
    }
}