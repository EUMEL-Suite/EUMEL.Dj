using System;
using Caliburn.Micro;
using Eumel.Dj.Ui.Core.Interfaces;
using TinyMessenger;

namespace Eumel.Dj.Ui.Core.ViewModels
{
    public class ChatViewModel : Screen, IChatViewModel
    {
        private readonly ITinyMessengerHub _hub;

        public ChatViewModel(ITinyMessengerHub hub)
        {
            _hub = hub ?? throw new ArgumentNullException(nameof(hub));
        }
    }
}