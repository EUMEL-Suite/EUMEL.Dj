using System;
using Caliburn.Micro;
using Eumel.Dj.Ui.Core.Interfaces;
using TinyMessenger;

namespace Eumel.Dj.Ui.Core.ViewModels
{
    public class ShellViewModel : Conductor<IScreen>.Collection.OneActive, IShellViewModel
    {
        public IStatusViewModel StatusWindow { get; }
        public IChatViewModel ChatWindow { get; }
        public ILogOutputViewModel LogWindow { get; }
        public ICurrentSongViewModel CurrentSongWindow { get; }
        public IPlayerViewModel PlayerWindow { get; }
        public IPlaylistViewModel PlaylistWindow { get; }

        private readonly ITinyMessengerHub _hub;
        private string _title;

        public string Title
        {
            get => _title;
            set
            {
                if (value == _title) return;
                _title = value;
                NotifyOfPropertyChange(() => Title);
            }
        }


        public ShellViewModel(ITinyMessengerHub hub,
            IStatusViewModel statusWindow,
            IChatViewModel chatWindow,
            ILogOutputViewModel logWindow,
            ICurrentSongViewModel currentSongWindow, 
            IPlaylistViewModel playlistWindow, 
            IPlayerViewModel playerWindow)
        {
            _hub = hub ?? throw new ArgumentNullException(nameof(hub));

            StatusWindow = statusWindow ?? throw new ArgumentNullException(nameof(statusWindow));
            ChatWindow = chatWindow ?? throw new ArgumentNullException(nameof(chatWindow));
            LogWindow = logWindow ?? throw new ArgumentNullException(nameof(logWindow));
            CurrentSongWindow = currentSongWindow ?? throw new ArgumentNullException(nameof(currentSongWindow));
            PlaylistWindow = playlistWindow ?? throw new ArgumentNullException(nameof(playlistWindow));
            PlayerWindow = playerWindow ?? throw new ArgumentNullException(nameof(playerWindow));

            Title = "EUMEL DJ Desktop";
        }
    }
}
