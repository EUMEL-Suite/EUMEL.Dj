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
        public const string PlaylistChanged = "PlaylistChanged";
    }
}