using System;
using System.Collections.Generic;
using Eumel.Dj.Core.Models;

namespace Eumel.Dj.Core
{
    public interface ISongsProviderService
    {
        IEnumerable<Song> GetSongs(int skip = 0, int take = int.MaxValue);
        Uri GetLocationOfSongById(string songId);
        Song FindSongById(string songId);
        SongsSource GetSourceInfo();
        IEnumerable<Song> SearchSongs(string query, int limit, out int numberOfSongs);
    }
}