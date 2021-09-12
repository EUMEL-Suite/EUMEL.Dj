using System.Threading.Tasks;
using Eumel.Dj.WebServer.Controllers;
using Microsoft.AspNetCore.SignalR;

namespace Eumel.Dj.WebServer.Hubs
{
    public class PlayerHub : Hub
    {
        public async Task PlayerStatusChanged(PlayerStatus status)
        {
            await Clients.All.SendAsync("PlayerStatusChanging", status);
        }
    }
}