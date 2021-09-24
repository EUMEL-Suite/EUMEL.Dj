using System;
using System.Collections.Generic;
using System.Linq;
using Eumel.Dj.Core;
using Eumel.Dj.Core.Models;
using TinyMessenger;

namespace Eumel.Dj.Ui.Extensions.PlaylistManager
{
    internal class SimplePlaylistManager : IDjPlaylistManager
    {
        private readonly IEnumerable<Song> _availableSongs;
        private readonly ITinyMessengerHub _hub;
        private readonly ISongsProviderService _songsService;
        private readonly Random _random;
        private readonly IList<VotedSong> _unvotedNext;
        private readonly IList<VotedSong> _votedSongs;

        public SimplePlaylistManager(IImplementationResolver<ISongsProviderService> playlistService, ITinyMessengerHub hub)
        {
            if (playlistService == null) throw new ArgumentNullException(nameof(playlistService));
            _hub = hub ?? throw new ArgumentNullException(nameof(hub));

            _songsService = playlistService.Resolve();
            _availableSongs = _songsService.GetSongs();
            _votedSongs = new List<VotedSong>();
            _random = new Random();
            _unvotedNext = Enumerable.Range(1, 10).Select(x =>
                _availableSongs.Skip(_random.Next(0, _availableSongs.Count() - 1)).First().ToVotedSong()).ToList();
        }

        public VotedSong GetTakeSong()
        {
            // take a voted song
            var result = _votedSongs.OrderByDescending(x => x.Voters.Count).FirstOrDefault();
            if (result != null)
            {
                _votedSongs.RemoveAt(0);
                return result;
            }

            // take an unvoted song
            result = _unvotedNext.First().ToVotedSong();
            _unvotedNext.RemoveAt(0);
            _unvotedNext.Add(_availableSongs.Skip(_random.Next(0, _availableSongs.Count() - 1)).First().ToVotedSong());
            return result;
        }

        public IEnumerable<Song> GetVotesFor(string votersName)
        {
            return _votedSongs.Where(x => x.Voters.Contains(votersName)).ToArray();
        }

        public void VoteUpFor(string votersName, Song song)
        {
            var votedSong = _votedSongs.SingleOrDefault(x => x.Id == song.Id);

            if (votedSong == null)
                _availableSongs.Where(x => x.Id == song.Id).ToList().ForEach(v =>
                {
                    var w = v.ToVotedSong(new[] { votersName });
                    _votedSongs.Add(w);
                });
            else if (!votedSong.Voters.Contains(votersName))
                votedSong.Voters.Add(votersName);
        }

        public void VoteDownFor(string votersName, Song song)
        {
            var votedSong = _votedSongs.SingleOrDefault(x => x.Id == song.Id);
            if (votedSong == null) return;

            if (votedSong.Voters.Contains(votersName))
                votedSong.Voters.Remove(votersName);
            if (!votedSong.Voters.Any())
                _votedSongs.Remove(votedSong);
        }

        public DjPlaylist GetPlaylist()
        {
            return new DjPlaylist(_votedSongs.OrderByDescending(x => x.Voters.Count).Concat(_unvotedNext).ToArray());
        }

        public void ClearVotesFor(string votersName)
        {
            var votedSongs = _votedSongs.Where(x => x.Voters.Contains(votersName)).ToArray();
            if (!votedSongs.Any()) return;

            votedSongs.ToList().ForEach(x => x.Voters = x.Voters.Where(y => y != votersName).ToArray());
        }
    }
}