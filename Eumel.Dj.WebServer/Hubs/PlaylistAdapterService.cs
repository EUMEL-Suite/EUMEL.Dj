using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Eumel.Dj.WebServer.Messages;
using Eumel.Dj.WebServer.Models;
using Microsoft.AspNetCore.SignalR;
using TinyMessenger;

namespace Eumel.Dj.WebServer.Hubs
{
    public class PlaylistAdapterService
    {

        private readonly IHubContext<PlaylistHub> _clientHub;

        private readonly List<TinyMessageSubscriptionToken> _tinyMessageSubscriptions;


        public PlaylistAdapterService(IHubContext<PlaylistHub> clientHub, ITinyMessengerHub serverHub)
        {
            _clientHub = clientHub;

            _tinyMessageSubscriptions = new List<TinyMessageSubscriptionToken>(new[]
            {
                _ = serverHub.Subscribe((Action<PlaylistChangedMessage>)(async (x) => await SendPlaylistChangedAsync(x.Playlist)))
            });
        }

        public Task SendPlaylistChangedAsync(DjPlaylist playlist)
        {
            return _clientHub.Clients.All.SendAsync(PlaylistHub.PlaylistChanged, playlist);
        }

    }
}