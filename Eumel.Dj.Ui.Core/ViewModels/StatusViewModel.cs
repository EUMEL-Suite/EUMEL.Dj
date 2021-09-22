using System;
using System.Collections.Generic;
using Caliburn.Micro;
using Eumel.Dj.Core.Messages;
using Eumel.Dj.Ui.Core.Interfaces;
using TinyMessenger;

namespace Eumel.Dj.Ui.Core.ViewModels
{
    public class StatusViewModel : Screen, IStatusViewModel, IDisposable
    {
        private readonly ITinyMessengerHub _hub;
        private readonly List<TinyMessageSubscriptionToken> _tinyMessageSubscriptions;
        private string _serviceStatus = ServiceStatusChangedMessage.ServiceStatus.Stopped.ToString();
        private string _playlistInfo = "Playlist '<unknown>' has 0 songs";
        private string _lastChatMessage = "Nothing to say";

        public string ServiceStatus
        {
            get => _serviceStatus;
            set
            {
                if (value == _serviceStatus) return;
                _serviceStatus = value;
                NotifyOfPropertyChange();
            }
        }

        public string PlaylistInfo
        {
            get => _playlistInfo;
            set
            {
                if (value == _playlistInfo) return;
                _playlistInfo = value;
                NotifyOfPropertyChange();
            }
        }

        public string LastChatMessage
        {
            get => _lastChatMessage;
            set
            {
                if (value == _lastChatMessage) return;
                _lastChatMessage = value;
                NotifyOfPropertyChange();
            }
        }

        public StatusViewModel(ITinyMessengerHub hub)
        {
            _hub = hub ?? throw new ArgumentNullException(nameof(hub));

            _tinyMessageSubscriptions = new List<TinyMessageSubscriptionToken>(new[]
            {
                _hub.Subscribe((Action<ServiceStatusChangedMessage>)ServiceStatusChanged)
            });
        }

        private void ServiceStatusChanged(ServiceStatusChangedMessage message)
        {
            ServiceStatus = message.Status.ToString();
        }

        public void Dispose()
        {
            _tinyMessageSubscriptions.ForEach(x => _hub.Unsubscribe(x));
        }
    }
}