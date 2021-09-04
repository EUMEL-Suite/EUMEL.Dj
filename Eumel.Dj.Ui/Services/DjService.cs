﻿using System;
using System.Collections.Generic;
using System.Windows.Media;
using Eumel.Dj.WebServer;
using Eumel.Dj.WebServer.Messages;
using Eumel.Dj.WebServer.Models;
using Microsoft.Extensions.Logging;
using TinyMessenger;

namespace Eumel.Dj.Ui.Services
{
    public class DjService : IDisposable
    {
        private readonly ITinyMessengerHub _hub;
        private readonly IPlaylistProviderService _playlistService;
        private VotedSong _currentSong;
        private readonly FixedSizedQueue<VotedSong> _pastSongs = new(5);
        private readonly MediaPlayer _mediaPlayer;
        private readonly DjList _djList;
        private readonly List<TinyMessageSubscriptionToken> _tinyMessageSubscriptions;

        public DjService(ITinyMessengerHub hub, IPlaylistProviderService playlistService)
        {
            _hub = hub;
            _playlistService = playlistService;
            _djList = new DjList(playlistService, hub);
            _tinyMessageSubscriptions = new List<TinyMessageSubscriptionToken>(new[]
            {
                hub.Subscribe((Action<PlayerMessage>)PlayerRequest),
                hub.Subscribe((Action<VoteMessage>)Vote),
                hub.Subscribe((Action<GetPlaylistMessage>)GetPlaylist),
                hub.Subscribe((Action<GetMyVotesMessage>)GetMyVotes)
            });

            _mediaPlayer = new MediaPlayer();
            _mediaPlayer.MediaEnded += (_, _) => PlayNextSong();
        }

        private void PlayNextSong(string directSongRequest = null)
        {
            if (_currentSong != null)
                _pastSongs.Enqueue(_currentSong);

            var notFoundCounter = 0;
            Uri songLocation = null;
            do
            {
                try
                {
                    _currentSong =_playlistService.FindSongById(directSongRequest).ToVotedSong()
                                   ?? _djList.GetTakeSong()
                                   ?? throw new Exception("cannot take song from list");
                    songLocation = _playlistService.GetLocationOfSongById(_currentSong.Id);
                }
                catch (SongNotFoundException ex)
                {
                    _hub.Publish(new LogMessage(this, ex.Message, LogLevel.Warning));
                    notFoundCounter++;
                    _currentSong = null;
                    songLocation = null;
                }
            } while (songLocation == null && notFoundCounter <= 10);


            _mediaPlayer.Open(songLocation);
            _mediaPlayer.Play();
        }

        private void GetMyVotes(GetMyVotesMessage message)
        {
            var result = _djList.GetVotesFor(message.VotersName);

            message.Response = new MessageResponse<IEnumerable<Song>>(result);
        }

        private void GetPlaylist(GetPlaylistMessage message)
        {
            var result = _djList.GetPlaylist();
            result.CurrentSong = _currentSong.ToVotedSong();
            result.PastSongs = _pastSongs.ToArray();

            message.Response = new MessageResponse<DjPlaylist>(result);
        }

        private void Vote(VoteMessage message)
        {
            switch (message.Direction)
            {
                case VoteMessage.UpDownVote.Up:
                    _djList.VoteUpFor(message.VotersName, message.Song);
                    break;
                case VoteMessage.UpDownVote.Down:
                    _djList.VoteDownFor(message.VotersName, message.Song);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void PlayerRequest(PlayerMessage message)
        {
            _mediaPlayer.Dispatcher.Invoke(() =>
            {
                switch (message.PlayerAction)
                {
                    case PlayerMessage.PlayerControl.Play:
                        // this needs to be changed to use the ID only!
                        PlayNextSong(message.SongId);
                        break;
                    case PlayerMessage.PlayerControl.Pause:
                        _mediaPlayer.Pause();
                        break;
                    case PlayerMessage.PlayerControl.Continue:
                        _mediaPlayer.Play();
                        break;
                    case PlayerMessage.PlayerControl.Stop:
                        _mediaPlayer.Stop();
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                message.Response = new MessageResponse<bool>(true);
            });
        }

        public void Dispose()
        {
            _tinyMessageSubscriptions.ForEach(x => _hub.Unsubscribe(x));

            _mediaPlayer.Stop();
            _mediaPlayer.Close();
        }
    }
}