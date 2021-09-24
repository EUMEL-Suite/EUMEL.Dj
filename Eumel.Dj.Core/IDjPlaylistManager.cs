using System.Collections.Generic;
using Eumel.Dj.Core.Models;

namespace Eumel.Dj.Core
{
    public interface IDjPlaylistManager
    {
        VotedSong GetTakeSong();
        IEnumerable<Song> GetVotesFor(string votersName);
        void VoteUpFor(string votersName, Song song);
        void VoteDownFor(string votersName, Song song);
        DjPlaylist GetPlaylist();
        void ClearVotesFor(string votersName);
    }
}