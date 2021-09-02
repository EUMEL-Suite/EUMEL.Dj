using System;

namespace Eumel.Dj.WebServer.Models
{
    public class Song
    {
        public string Name { get; set; }
        public string Album { get; set; }
        public string Artist { get; set; }
        public string AlbumArtist { get; set; }
        [Obsolete("This will be replaced by persistent id to avoid 'mp3 injection'")]
        public string Location { get; set; }
        public string Id { get; set; }
    }
}