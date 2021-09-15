using System.Diagnostics;
using System.Threading.Tasks;
using Eumel.Dj.WebServer.Exceptions;
using Microsoft.AspNetCore.SignalR;

namespace Eumel.Dj.WebServer.Hubs
{
    public class ChatHub : Hub
    {
        public ChatHub()
        {
            // this is to make sure, the constant for the hub message match the method names
            Debug.Assert(nameof(SendChat) == Constants.ChatHub.SendChat, message: "Method name must be added with the same name to constants");
        }

        public async Task SendChat(string username, string message)
        {
            if (!Context.GetHttpContext().Request.Headers.TryGetValue(Constants.UserToken, out var usertoken))
                return;

            // todo validate token through token service

            await Clients.All.SendAsync(Constants.ChatHub.ChatSent, username, message);
        }
    }
}
