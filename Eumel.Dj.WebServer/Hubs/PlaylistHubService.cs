using System;
using System.Threading.Tasks;
using Eumel.Dj.WebServer.Messages;
using Eumel.Dj.WebServer.Models;
using Microsoft.AspNetCore.SignalR;
using TinyMessenger;

namespace Eumel.Dj.WebServer.Hubs
{
    public class PlaylistHubService
    {
        private readonly IHubContext<PlaylistHub> _clientHub;

        public PlaylistHubService(IHubContext<PlaylistHub> clientHub, ITinyMessengerHub serverHub)
        {
            _clientHub = clientHub;

            _ = serverHub.Subscribe((Action<PlaylistChangedMessage>)(async (x) => await SendPlaylistChangedAsync(x.Playlist)));
        }

        public Task SendPlaylistChangedAsync(DjPlaylist playlist)
        {
            return _clientHub.Clients.All.SendAsync("PlaylistChanged", playlist);
        }
    }
}