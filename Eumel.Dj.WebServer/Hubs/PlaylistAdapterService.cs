using System;
using System.Threading.Tasks;
using Eumel.Dj.Core.Messages;
using Eumel.Dj.Core.Models;
using Microsoft.AspNetCore.SignalR;
using TinyMessenger;

namespace Eumel.Dj.WebServer.Hubs
{
    public class PlaylistAdapterService : AdapterServiceBase<PlaylistHub>
    {
        public PlaylistAdapterService(IHubContext<PlaylistHub> clientHub, ITinyMessengerHub applicationHub) : base(clientHub, applicationHub)
        {
            // connect to application bus and send messages to signalr hub
            Subscribe((Action<PlaylistChangedMessage>)(async (x) => await SendPlaylistChangedAsync(x.Playlist)));
        }

        private Task SendPlaylistChangedAsync(DjPlaylist playlist)
        {
            return ClientHub.Clients.All.SendAsync(Constants.PlaylistHub.PlaylistChanged, playlist);
        }
    }
}