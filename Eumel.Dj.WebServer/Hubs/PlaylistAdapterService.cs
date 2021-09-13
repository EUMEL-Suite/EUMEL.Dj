﻿using System;
using System.Threading.Tasks;
using Eumel.Dj.WebServer.Messages;
using Eumel.Dj.WebServer.Models;
using Microsoft.AspNetCore.SignalR;
using TinyMessenger;

namespace Eumel.Dj.WebServer.Hubs
{
    public class PlaylistAdapterService : AdapterServiceBase
    {
        public PlaylistAdapterService(IHubContext<PlaylistHub> clientHub, ITinyMessengerHub serverHub) : base(clientHub, serverHub)
        {
            Subscribe((Action<PlaylistChangedMessage>)(async (x) => await SendPlaylistChangedAsync(x.Playlist)));
        }

        private Task SendPlaylistChangedAsync(DjPlaylist playlist)
        {
            return ClientHub.Clients.All.SendAsync(PlaylistHub.PlaylistChanged, playlist);
        }
    }
}