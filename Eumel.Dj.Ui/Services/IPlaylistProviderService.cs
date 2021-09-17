using System;
using System.Collections.Generic;
using Eumel.Dj.Core.Models;

namespace Eumel.Dj.Ui.Services
{
    public interface IPlaylistProviderService
    {
        IEnumerable<Song> GetSongs(int skip = 0, int take = int.MaxValue);
        Uri GetLocationOfSongById(string songId);
        Song FindSongById(string songId);
    }
}