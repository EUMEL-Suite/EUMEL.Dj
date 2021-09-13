using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using TinyMessenger;

namespace Eumel.Dj.WebServer.Hubs
{
    public class ChatHub : Hub
    {
        public async Task SendChatMessage(string username, string message)
        {
            await Clients.All.SendAsync("MessageReceived", new ChatHubMessage(username, message));
        }
    }
}
