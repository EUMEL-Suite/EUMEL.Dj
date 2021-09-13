using Microsoft.AspNetCore.SignalR;

namespace Eumel.Dj.WebServer.Hubs
{
    public class PlaylistHub : Hub
    {
        public const string Route = "playlistHub";
        public const string PlaylistChanged = "PlaylistChanged";
    }
}