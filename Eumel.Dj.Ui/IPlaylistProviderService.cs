﻿using System.Collections.Generic;
using Eumel.Dj.WebServer.Models;

namespace Eumel.Dj.Ui
{
    public interface IPlaylistProviderService
    {
        IEnumerable<Song> GetSongs(int skip = 0, int take = int.MaxValue);
    }
}