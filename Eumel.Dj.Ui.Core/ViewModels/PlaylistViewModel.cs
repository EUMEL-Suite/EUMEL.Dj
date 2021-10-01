using System;
using System.Collections.ObjectModel;
using System.Linq;
using Caliburn.Micro;
using Eumel.Core.Extensions;
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
            message.Playlist.PastSongs?.ForEach(Songs.Add);

            if (message.Playlist.CurrentSong != null)
                Songs.Add(message.Playlist.CurrentSong);

            message.Playlist.UpcomingSongs?.ForEach(Songs.Add);
        }
    }
}