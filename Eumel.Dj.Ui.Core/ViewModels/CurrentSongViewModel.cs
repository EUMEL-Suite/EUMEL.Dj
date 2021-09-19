using System;
using Caliburn.Micro;
using Eumel.Dj.Ui.Core.Interfaces;
using TinyMessenger;

namespace Eumel.Dj.Ui.Core.ViewModels
{
    public class CurrentSongViewModel : Screen, ICurrentSongViewModel
    {
        private readonly ITinyMessengerHub _hub;

        public CurrentSongViewModel(ITinyMessengerHub hub)
        {
            _hub = hub ?? throw new ArgumentNullException(nameof(hub));
        }
    }
}