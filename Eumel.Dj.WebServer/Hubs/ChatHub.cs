using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using TinyMessenger;

namespace Eumel.Dj.WebServer.Hubs
{
    public class ChatHub : Hub
    {
        // ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable
        private readonly ITinyMessengerHub _hub;

        public ChatHub(ITinyMessengerHub hub)
        {
            _hub = hub;

            _hub.Subscribe((Action<ChatMessage>)(async (x) => await SendChatMessage(x.Username, x.Message)));
        }

        public async Task SendChatMessage(string username, string message)
        {
            await Clients.All.SendAsync("MessageReceived", new ChatHubMessage(username, message));
        }
    }
}
