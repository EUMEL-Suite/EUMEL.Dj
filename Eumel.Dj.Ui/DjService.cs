using System;
using System.Collections.Generic;
using System.Windows.Media;
using Eumel.Dj.WebServer.Controllers;
using Eumel.Dj.WebServer.Messages;
using Eumel.Dj.WebServer.Models;
using TinyMessenger;

namespace Eumel.Dj.Ui
{
    public class DjService : IDisposable
    {
        private readonly IPlaylistProviderService _playlistService;
        private readonly MediaPlayer _mediaPlayer;
        private readonly DjList _djList;

        public DjService(ITinyMessengerHub hub, IPlaylistProviderService playlistService)
        {
            _playlistService = playlistService;
            _djList = new DjList(playlistService);

            hub.Subscribe((Action<PlayerMessage>)PlayerRequest);
            hub.Subscribe((Action<VoteMessage>)Vote);
            hub.Subscribe((Action<GetPlaylistMessage>)GetPlaylist);
            hub.Subscribe((Action<GetMyVotesMessage>)GetMyVotes);

            _mediaPlayer = new MediaPlayer();
            _mediaPlayer.MediaEnded += (sender, e) =>
            {
                _mediaPlayer.Open(new Uri(_djList.GetTakeSong().Location));
                _mediaPlayer.Play();
            };
        }

        private void GetMyVotes(GetMyVotesMessage message)
        {
            var result = _djList.GetVotesFor(message.VotersName);

            message.Response = new MessageResponse<IEnumerable<Song>>(result);
        }

        private void GetPlaylist(GetPlaylistMessage message)
        {
            var result = _djList.GetPlaylist();

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
                        if (!string.IsNullOrWhiteSpace(message.Location))
                            _mediaPlayer.Open(new Uri(message.Location));
                        if (!_mediaPlayer.HasAudio)
                            _mediaPlayer.Open(new Uri(_djList.GetTakeSong().Location));
                        _mediaPlayer.Play();
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
            _mediaPlayer.Stop();
            _mediaPlayer.Close();
        }
    }
}