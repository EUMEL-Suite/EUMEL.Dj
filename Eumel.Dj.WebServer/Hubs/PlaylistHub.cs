using System;
using System.Threading.Tasks;
using Eumel.Dj.WebServer.Messages;
using Eumel.Dj.WebServer.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using TinyMessenger;

namespace Eumel.Dj.WebServer.Hubs
{
    public class PlaylistHub : Hub
    {
        // ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable
        private readonly ITinyMessengerHub _hub;

        public PlaylistHub()
        {
            _hub = TinyMessengerHub.DefaultHub;
            _hub.Publish(new LogMessage(this, "Playlist Hub Started", LogLevel.Information));

            _hub.Subscribe((Action<PlaylistChangedMessage>)(async (x) => await PlaylistChangedAsync(x.Playlist)));
        }

        public PlaylistHub(ITinyMessengerHub hub)
        {
            _hub = hub;
            _hub.Publish(new LogMessage(this, "Playlist Hub Started", LogLevel.Information));

            _hub.Subscribe((Action<PlaylistChangedMessage>)(async (x) => await PlaylistChangedAsync(x.Playlist)));
        }

        public async Task PlaylistChangedAsync(DjPlaylist playlist)
        {
            await Clients.All.SendAsync("PlaylistChanged", playlist);
        }
    }
}