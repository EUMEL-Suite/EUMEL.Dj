using System;
using System.Collections.Generic;
using System.Linq;
using Eumel.Dj.WebServer.Messages;
using Eumel.Dj.WebServer.Models;
using Microsoft.Extensions.Logging;
using TinyMessenger;

namespace Eumel.Dj.Ui.Services
{
    public class DjList
    {
        private readonly IPlaylistProviderService _playlistService;
        private readonly ITinyMessengerHub _hub;
        private readonly IEnumerable<Song> _availableSongs;
        private readonly IList<VotedSong> _votedSongs;
        private readonly IList<Song> _unvotedNext;
        private readonly Random _random;

        public DjList(IPlaylistProviderService playlistService, ITinyMessengerHub hub)
        {
            _playlistService = playlistService;
            _hub = hub;
            _availableSongs = playlistService.GetSongs();
            _votedSongs = new List<VotedSong>();
            _random = new Random();
            _unvotedNext = Enumerable.Range(1, 10).Select(x => _availableSongs.Skip(_random.Next(0, _availableSongs.Count() - 1)).First()).ToList();
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
            _unvotedNext.Add(_availableSongs.Skip(_random.Next(0, _availableSongs.Count() - 1)).First());
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
                _votedSongs.Add(song.ToVotedSong(new[] { votersName }));
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
            return new DjPlaylist(_votedSongs.OrderByDescending(x => x.Voters.Count).ToArray());
        }
    }
}