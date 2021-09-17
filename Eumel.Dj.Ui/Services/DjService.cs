using System;
using System.Collections.Generic;
using System.Windows.Media;
using Eumel.Dj.Core.Exceptions;
using Eumel.Dj.Core.Messages;
using Eumel.Dj.Core.Models;
using Eumel.Dj.WebServer.Controllers;
using Microsoft.Extensions.Logging;
using TinyMessenger;

namespace Eumel.Dj.Ui.Services
{
    public class DjService : IDisposable
    {
        private readonly DjList _djList;
        private readonly ITinyMessengerHub _hub;
        private readonly MediaPlayer _mediaPlayer;
        private readonly FixedSizedQueue<VotedSong> _pastSongs = new(3);
        private readonly IPlaylistProviderService _playlistService;
        private readonly List<TinyMessageSubscriptionToken> _tinyMessageSubscriptions;
        private VotedSong _currentSong;
        private PlayerStatus _playerStatus = Dj.Core.Messages.PlayerStatus.Stopped;

        public DjService(ITinyMessengerHub hub, IPlaylistProviderService playlistService)
        {
            _hub = hub;
            _playlistService = playlistService;
            _djList = new DjList(playlistService, hub);
            _tinyMessageSubscriptions = new List<TinyMessageSubscriptionToken>(new[]
            {
                hub.Subscribe((Action<PlayerMessage>)PlayerRequest),
                hub.Subscribe((Action<VoteMessage>)Vote),
                hub.Subscribe((Action<ClearMyVotesMessage>)ClearMyVotes),
                hub.Subscribe((Action<GetPlaylistMessage>)GetPlaylist),
                hub.Subscribe((Action<GetMyVotesMessage>)GetMyVotes),
                hub.Subscribe((Action<PlayerStatusMessage>)PlayerStatus)
            });

            _mediaPlayer = new MediaPlayer();
            _mediaPlayer.MediaEnded += (_, _) => PlayNextSong();
        }

        public void Dispose()
        {
            _tinyMessageSubscriptions.ForEach(x => _hub.Unsubscribe(x));

            _mediaPlayer.Stop();
            _mediaPlayer.Close();
        }

        private void ClearMyVotes(ClearMyVotesMessage message)
        {
            _djList.ClearVotesFor(message.Username);
        }

        private void PlayerStatus(PlayerStatusMessage message)
        {
            message.Response = new MessageResponse<PlayerStatus>(_playerStatus);
        }

        private void ContinueOrNext()
        {
            if (_mediaPlayer.Source == null)
            {
                _currentSong = _djList.GetTakeSong();
                var location = _playlistService.GetLocationOfSongById(_currentSong.Id);
                _mediaPlayer.Open(location);

                var playlist = _djList.GetPlaylist();
                playlist.CurrentSong = _currentSong;
                playlist.PastSongs = _pastSongs.ToArray();
                _hub.Publish(new PlaylistChangedMessage(this, playlist));
            }

            _mediaPlayer.Play();
        }

        private void PlayNextSong()
        {
            if (_currentSong != null)
                _pastSongs.Enqueue(_currentSong);

            var notFoundCounter = 0;
            Uri songLocation;
            do
            {
                try
                {
                    _currentSong = _djList.GetTakeSong() ?? throw new Exception("cannot take song from list");
                    songLocation = _playlistService.GetLocationOfSongById(_currentSong.Id);
                }
                catch (SongNotFoundDjException ex)
                {
                    _hub.Publish(new LogMessage(this, ex.Message, LogLevel.Warning));
                    notFoundCounter++;
                    _currentSong = null;
                    songLocation = null;
                }
            } while (songLocation == null && notFoundCounter <= 10);

            _mediaPlayer.Open(songLocation);
            _mediaPlayer.Play();
            var playlist = _djList.GetPlaylist();
            playlist.CurrentSong = _currentSong;
            playlist.PastSongs = _pastSongs.ToArray();
            _hub.Publish(new PlaylistChangedMessage(this, playlist));
        }

        private void GetMyVotes(GetMyVotesMessage message)
        {
            var result = _djList.GetVotesFor(message.VotersName);

            message.Response = new MessageResponse<IEnumerable<Song>>(result);
        }

        private void GetPlaylist(GetPlaylistMessage message)
        {
            var result = _djList.GetPlaylist();
            result.CurrentSong = _currentSong;
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
                        ContinueOrNext();
                        _playerStatus = Dj.Core.Messages.PlayerStatus.Playing;
                        break;
                    case PlayerMessage.PlayerControl.Pause:
                        _mediaPlayer.Pause();
                        _playerStatus = Dj.Core.Messages.PlayerStatus.Paused;
                        break;
                    case PlayerMessage.PlayerControl.Stop:
                        _mediaPlayer.Stop();
                        _mediaPlayer.Position = TimeSpan.Zero;
                        _playerStatus = Dj.Core.Messages.PlayerStatus.Stopped;
                        break;
                    case PlayerMessage.PlayerControl.Next:
                        PlayNextSong();
                        break;
                    case PlayerMessage.PlayerControl.Restart:
                        _mediaPlayer.Position = TimeSpan.Zero;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                message.Response = new MessageResponse<bool>(true);
            });
        }
    }
}