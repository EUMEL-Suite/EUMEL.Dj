using System;
using System.Collections.ObjectModel;
using System.Linq;
using Caliburn.Micro;
using Eumel.Dj.Core.Messages;
using Eumel.Dj.Core.Models;
using Eumel.Dj.Ui.Core.Interfaces;
using TinyMessenger;

namespace Eumel.Dj.Ui.Core.ViewModels
{
    public class PlaylistViewModel : Screen, IPlaylistViewModel
    {
        private readonly ITinyMessengerHub _hub;

        public ObservableCollection<VotedSong> Songs { get; } = new();


        public PlaylistViewModel(ITinyMessengerHub hub)
        {
            _hub = hub ?? throw new ArgumentNullException(nameof(hub));

            _hub.Subscribe((Action<PlaylistChangedMessage>)PlaylistChanged);
        }

        private void PlaylistChanged(PlaylistChangedMessage message)
        {
            Songs.Clear();
            if (message.Playlist.PastSongs != null)
                message.Playlist.PastSongs.ToList().ForEach(Songs.Add);

            if (message.Playlist.CurrentSong != null)
                Songs.Add(message.Playlist.CurrentSong);

            if (message.Playlist.UpcomingSongs != null)
                message.Playlist.UpcomingSongs.ToList().ForEach(Songs.Add);
        }
    }
}