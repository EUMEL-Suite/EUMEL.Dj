using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.SignalR;
using TinyMessenger;

namespace Eumel.Dj.WebServer.Hubs
{
    /// <summary>
    /// base class for bus to bus adapter. base class takes care of subscription token and removes subscriptions on dispose
    /// </summary>
    public abstract class AdapterServiceBase<T> : IDisposable
        where T: Hub
    {
        private readonly ITinyMessengerHub _serverHub;
        private readonly List<TinyMessageSubscriptionToken> _tinyMessageSubscriptions = new();
        protected IHubContext<T> ClientHub { get; }

        protected AdapterServiceBase(IHubContext<T> clientHub, ITinyMessengerHub serverHub)
        {
            _serverHub = serverHub;
            ClientHub = clientHub;
        }

        protected void Subscribe<TMessage>(Action<TMessage> deliveryAction) where TMessage : class, ITinyMessage
        {
            _tinyMessageSubscriptions.Add(_serverHub.Subscribe(deliveryAction));
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _tinyMessageSubscriptions?.ForEach(x=>_serverHub.Unsubscribe(x));
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}