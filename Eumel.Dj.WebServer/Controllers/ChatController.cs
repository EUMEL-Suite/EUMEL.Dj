using System;
using Eumel.Dj.WebServer.Hubs;
using Eumel.Dj.WebServer.Messages;
using Eumel.Dj.WebServer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TinyMessenger;

namespace Eumel.Dj.WebServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : EumelDjControllerBase
    {
        private readonly ITinyMessengerHub _hub;

        public ChatController(ITinyMessengerHub hub, ITokenService tokenService) : base(tokenService)
        {
            _hub = hub ?? throw new ArgumentNullException(nameof(hub));
        }

        [HttpPost("SendMessage")]
        public bool SendMessage([FromBody] string message)
        {
            if (string.IsNullOrWhiteSpace(Username))
            {
                _hub.Publish(new LogMessage(this, $"Unknown user tries to access REST api {nameof(ChatController)}.{nameof(SendMessage)}. Source: {GetClientIp()}", LogLevel.Warning));
                return false;
            }
            _hub.Publish(new ChatMessage(this, Username, message));

            return true;
        }
    }
}