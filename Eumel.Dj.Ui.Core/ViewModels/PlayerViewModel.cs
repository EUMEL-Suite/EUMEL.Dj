using System;
using Caliburn.Micro;
using Eumel.Dj.Core.Messages;
using Eumel.Dj.Ui.Core.Interfaces;
using TinyMessenger;

namespace Eumel.Dj.Ui.Core.ViewModels
{
    public class PlayerViewModel : Screen, IPlayerViewModel
    {
        private readonly ITinyMessengerHub _hub;

        public PlayerViewModel(ITinyMessengerHub hub)
        {
            _hub = hub ?? throw new ArgumentNullException(nameof(hub));
        }

        public void Play()
        {
            _hub.Publish(new PlayerMessage(this, PlayerMessage.PlayerControl.Play));
        }

        public void Pause()
        {
            _hub.Publish(new PlayerMessage(this, PlayerMessage.PlayerControl.Pause));
        }

        public void Next()
        {
            _hub.Publish(new PlayerMessage(this, PlayerMessage.PlayerControl.Next));
        }

        public void Stop()
        {
            _hub.Publish(new PlayerMessage(this, PlayerMessage.PlayerControl.Stop));
        }

        public void Restart()
        {
            _hub.Publish(new PlayerMessage(this, PlayerMessage.PlayerControl.Restart));
        }
    }
}