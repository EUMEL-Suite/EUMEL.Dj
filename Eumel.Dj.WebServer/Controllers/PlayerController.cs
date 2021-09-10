using System;
using Eumel.Dj.WebServer.Messages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TinyMessenger;

namespace Eumel.Dj.WebServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlayerController : EumelDjControllerBase
    {
        private readonly ITinyMessengerHub _hub;

        public PlayerController(ITinyMessengerHub hub)
        {
            _hub = hub ?? throw new ArgumentNullException(nameof(hub));
        }

        [HttpGet("Play")]
        public bool Play()
        {
            _hub.Publish(new LogMessage(this, $"User with token {Usertoken} requested player to play", LogLevel.Information));

            var message = new PlayerMessage(this, PlayerMessage.PlayerControl.Play);
            _hub.Publish(message);

            if (!message.Response.Success)
                throw new Exception(message.Response.ErrorMessage);

            return true;
        }

        [HttpGet("Open")]
        public bool Open(string songId)
        {
            _hub.Publish(new LogMessage(this, $"User with token {Usertoken} requested player to open", LogLevel.Information));

            var message = new PlayerMessage(this, PlayerMessage.PlayerControl.Play, songId);
            _hub.Publish(message);

            if (!message.Response.Success)
                throw new Exception(message.Response.ErrorMessage);

            return true;
        }

        [HttpGet("Stop")]
        public bool Stop()
        {
            _hub.Publish(new LogMessage(this, $"User with token {Usertoken} requested player to stop", LogLevel.Information));

            var message = new PlayerMessage(this, PlayerMessage.PlayerControl.Stop);
            _hub.Publish(message);

            if (!message.Response.Success)
                throw new Exception(message.Response.ErrorMessage);

            return true;
        }

        [HttpGet("Pause")]
        public bool Pause()
        {
            _hub.Publish(new LogMessage(this, $"User with token {Usertoken} requested player to pause", LogLevel.Information));

            var message = new PlayerMessage(this, PlayerMessage.PlayerControl.Pause);
            _hub.Publish(message);

            if (!message.Response.Success)
                throw new Exception(message.Response.ErrorMessage);

            return true;
        }

        [HttpGet("Continue")]
        public bool Continue()
        {
            _hub.Publish(new LogMessage(this, $"User with token {Usertoken} requested player to continue", LogLevel.Information));

            var message = new PlayerMessage(this, PlayerMessage.PlayerControl.Continue);
            _hub.Publish(message);

            if (!message.Response.Success)
                throw new Exception(message.Response.ErrorMessage);

            return true;
        }

        [HttpGet("Status")]
        public PlayerMessage.PlayerControl Status()
        {
            var message = new PlayerStatusMessage(this);
            _hub.Publish(message);

            return message.Response.Response;
        }
    }
}