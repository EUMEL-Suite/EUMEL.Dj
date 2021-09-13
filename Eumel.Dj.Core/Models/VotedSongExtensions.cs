using System.Collections.Generic;
using System.Linq;

namespace Eumel.Dj.WebServer.Models
{
    public static class VotedSongExtensions
    {
        public static VotedSong ToVotedSong(this Song song, IEnumerable<string> voters = null)
        {
            if (song == null) return null;

            return new VotedSong()
            {
                Id = song.Id,
                Album = song.Album,
                AlbumArtist = song.AlbumArtist,
                Artist = song.Artist,
                Name = song.Name,
                Voters = new List<string>(voters ?? Enumerable.Empty<string>())
            };
        }
    }
}