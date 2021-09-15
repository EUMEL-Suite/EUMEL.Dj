using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Client;
using TinyMessenger;

namespace Eumel.Dj.WebServer.Hubs
{
    /// <summary>
    /// base class for bus to bus adapter. base class takes care of subscription token and removes subscriptions on dispose
    /// </summary>
    public abstract class AdapterServiceBase<T> : IDisposable
        where T: Hub
    {
        private readonly ITinyMessengerHub _applicationHub;
        private readonly List<TinyMessageSubscriptionToken> _tinyMessageSubscriptions = new();
        protected  HubConnection HubConnection { get; private set; }
        protected IHubContext<T> ClientHub { get; }

        protected AdapterServiceBase(IHubContext<T> clientHub, ITinyMessengerHub applicationHub)
        {
            _applicationHub = applicationHub;
            ClientHub = clientHub;
        }

        protected void Subscribe<TMessage>(Action<TMessage> deliveryAction) where TMessage : class, ITinyMessage
        {
            _tinyMessageSubscriptions.Add(_applicationHub.Subscribe(deliveryAction));
        }

        protected virtual async void Dispose(bool disposing)
        {
            if (!disposing) return;

            if (HubConnection != null)
            {
                await HubConnection.StopAsync();
                await HubConnection.DisposeAsync();
            }

            _tinyMessageSubscriptions?.ForEach(x => _applicationHub.Unsubscribe(x));
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected void CreateHubConnection(string hubEndpoint)
        {
            HubConnection = new HubConnectionBuilder()
                .WithUrl(hubEndpoint, options =>
                {
                    options.Headers.Add(Constants.UserToken, Guid.NewGuid().ToString());
                    options.HttpMessageHandlerFactory = message =>
                    {
                        if (message is HttpClientHandler clientHandler)
                            // always ignore the SSL certificate
                            clientHandler.ServerCertificateCustomValidationCallback +=
                                (sender, certificate, chain, sslPolicyErrors) => true;
                        return message;
                    };
                })
                .Build();

            HubConnection.Closed += async (error) =>
            {
                await Task.Delay(new Random().Next(0, 5) * 1000);
                await HubConnection.StartAsync();
            };
            HubConnection.StartAsync();
        }
    }
}