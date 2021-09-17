using System;
using System.Net.Http;
using System.Threading.Tasks;
using Eumel.Dj.Core.Messages;
using Eumel.Dj.Core.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Client;
using TinyMessenger;

namespace Eumel.Dj.WebServer.Hubs
{
    public class ChatAdapterService : AdapterServiceBase<ChatHub>
    {
        public ChatAdapterService(IHubContext<ChatHub> clientHub, ITinyMessengerHub applicationHub, IAppSettings settings) : base(clientHub, applicationHub)
        {
            Subscribe((Action<ChatSendingMessage>)(async (x) => await SendChatSentAsync(x)));

            // this adapter need to listen to the bus itself because it represents a bidirectional interface
            InitHubConnection(applicationHub, settings);
        }

        protected void InitHubConnection(ITinyMessengerHub serverHub, IAppSettings settings)
        {
            // creates a hub connection so it can be used
            CreateHubConnection($"{settings.RestEndpoint}/{Constants.ChatHub.Route}");

            HubConnection.On<string, string>(Constants.ChatHub.ChatSent, (username, message) => { serverHub.Publish(new ChatReceivedMessage(this, username, message)); });
        }

        private async Task SendChatSentAsync(ChatSendingMessage message)
        {
            await ClientHub.Clients.All.SendAsync(Constants.ChatHub.ChatSent, message.Username, message.Message);
        }
    }
}