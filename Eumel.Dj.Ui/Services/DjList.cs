using System;
using System.Collections.Generic;
using System.Linq;
using Eumel.Dj.WebServer.Models;

namespace Eumel.Dj.Ui.Services
{
    public class DjList : IDisposable
    {
        private readonly IPlaylistProviderService _playlistService;
        private readonly IEnumerable<Song> _availableSongs;
        private readonly IList<VotedSong> _votedSongs;
        private readonly IList<Song> _unvotedNext;
        private readonly Random _random;

        public DjList(IPlaylistProviderService playlistService)
        {
            _playlistService = playlistService;
            _availableSongs = playlistService.GetSongs();
            _votedSongs = new List<VotedSong>();
            _random = new Random();
            _unvotedNext = Enumerable.Range(1, 10).Select(x => _availableSongs.Skip(_random.Next(0, _availableSongs.Count() - 1)).First()).ToList();
        }

        public class VotedSong
        {
            public VotedSong(Song song)
            {
                Song = song;
            }

            public Song Song { get; }
            public List<string> Voters { get; } = new();
        }

        public Song GetTakeSong()
        {
            // take a voted song
            var result = _votedSongs.OrderByDescending(x => x.Voters.Count).FirstOrDefault()?.Song;
            if (result != null)
            {
                _votedSongs.RemoveAt(0);
                return result;
            }
            // take an unvoted song
            result = _unvotedNext.First();
            _unvotedNext.RemoveAt(0);
            _unvotedNext.Add(_availableSongs.Skip(_random.Next(0, _availableSongs.Count() - 1)).First());
            return result;
        }

        public IEnumerable<Song> GetVotesFor(string votersName)
        {
            return _votedSongs.Where(x => x.Voters.Contains(votersName)).Select(x => x.Song).ToArray();
        }

        public void VoteUpFor(string votersName, Song song)
        {
            var votedSong = _votedSongs.SingleOrDefault(x => x.Song.Location == song.Location);

            if (votedSong == null)
            {
                var vs = new VotedSong(song);
                vs.Voters.Add(votersName);
                _votedSongs.Add(vs);
            }
            else if (!votedSong.Voters.Contains(votersName))
                votedSong.Voters.Add(votersName);
        }

        public void VoteDownFor(string votersName, Song song)
        {
            var votedSong = _votedSongs.SingleOrDefault(x => x.Song.Location == song.Location);
            if (votedSong == null) return;

            if (votedSong.Voters.Contains(votersName))
                votedSong.Voters.Remove(votersName);
            if (!votedSong.Voters.Any())
                _votedSongs.Remove(votedSong);
        }

        public DjPlaylist GetPlaylist()
        {
            return new DjPlaylist(
                _votedSongs.OrderByDescending(x => x.Voters.Count).Select(x => new WebServer.Models.VotedSong(x.Song, x.Voters.Count)),
                _unvotedNext);
        }

        public void Dispose()
        {
        }
    }
}