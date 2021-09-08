using System.Collections.Generic;

namespace Eumel.Dj.Mobile.Models
{
    public class PlaylistItem
    {
        public IEnumerable<PlaylistSongItem> Songs { get; set; }
    }
}