using System.Collections.Generic;

namespace Eumel.Dj.Mobile.Models
{
    public class SongListItem
    {
        public string Name { get; set; }
        public int NumberOfSongs { get; set; }

        public IEnumerable<SongItem> Songs { get; set; }
    }
}