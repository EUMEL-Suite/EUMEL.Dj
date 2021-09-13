using System;
using System.Threading.Tasks;
using Eumel.Dj.WebServer.Messages;
using Eumel.Dj.WebServer.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using TinyMessenger;

namespace Eumel.Dj.WebServer.Hubs
{
    // todo strongly tyed hub
    public class PlaylistHub : Hub
    {
        // ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable
        private readonly ITinyMessengerHub _hub;

        public PlaylistHub(ITinyMessengerHub hub)
        {
            _hub = hub;
            _hub.Publish(new LogMessage(this, "Playlist Hub Started", LogLevel.Information));
        }

        // do i need this for one directional?
        public async Task PlaylistChangedAsync(DjPlaylist playlist)
        {
            await Clients.All.SendAsync("PlaylistChanged", playlist);
        }
    }
}