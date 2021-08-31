namespace Eumel.Dj.WebServer.Models
{
    public class VotedSong : Song
    {
        public VotedSong(Song song, int votes)
        {
            Album = song.Album;
            AlbumArtist = song.AlbumArtist;
            Artist = song.Artist;
            Location = song.Location;
            Name = song.Name;
            Votes = votes;
        }

        public int Votes { get; set; }
    }
}