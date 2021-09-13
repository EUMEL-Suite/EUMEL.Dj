using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace Eumel.Dj.WebServer.Hubs
{
    public class ChatHub : Hub
    {
        public const string Route = "chatHub";
        public const string ChatSent = "ChatSent";

        public async Task SendChatMessage(string username, string message)
        {
            string usertoken = null;
            if (Context.GetHttpContext().Request.Headers.ContainsKey("usertoken"))
                usertoken = Context.GetHttpContext().Request.Headers["usertoken"];

            if (usertoken == null)
                return; // dont forward without token

            await Clients.All.SendAsync(ChatSent, username, message);
        }
    }
}
