using System;
using System.Collections.Generic;
using System.Linq;
using Eumel.Dj.WebServer.Models;
using TinyMessenger;

namespace Eumel.Dj.Ui.Services
{
    public class DjList
    {
        private readonly IEnumerable<Song> _availableSongs;
        private readonly ITinyMessengerHub _hub;
        private readonly IPlaylistProviderService _playlistService;
        private readonly Random _random;
        private readonly IList<VotedSong> _unvotedNext;
        private readonly IList<VotedSong> _votedSongs;

        public DjList(IPlaylistProviderService playlistService, ITinyMessengerHub hub)
        {
            _playlistService = playlistService;
            _hub = hub;
            _availableSongs = playlistService.GetSongs();
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
    }
}