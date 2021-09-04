using System;
using Eumel.Dj.WebServer.Messages;
using Microsoft.AspNetCore.Mvc;
using TinyMessenger;

namespace Eumel.Dj.WebServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlayerController : ControllerBase
    {
        private readonly ITinyMessengerHub _hub;

        public PlayerController(ITinyMessengerHub hub)
        {
            _hub = hub ?? throw new ArgumentNullException(nameof(hub));
        }

        [HttpGet("Play")]
        public bool Play()
        {
            var message = new PlayerMessage(this, PlayerMessage.PlayerControl.Play);
            _hub.Publish(message);

            if (!message.Response.Success)
                throw new Exception(message.Response.ErrorMessage);

            return true;
        }

        [HttpGet("Open")]
        public bool Open(string songId)
        {
            var message = new PlayerMessage(this, PlayerMessage.PlayerControl.Play, songId);
            _hub.Publish(message);

            if (!message.Response.Success)
                throw new Exception(message.Response.ErrorMessage);

            return true;
        }

        [HttpGet("Stop")]
        public bool Stop()
        {
            var message = new PlayerMessage(this, PlayerMessage.PlayerControl.Stop);
            _hub.Publish(message);

            if (!message.Response.Success)
                throw new Exception(message.Response.ErrorMessage);

            return true;
        }

        [HttpGet("Pause")]
        public bool Pause()
        {
            var message = new PlayerMessage(this, PlayerMessage.PlayerControl.Pause);
            _hub.Publish(message);

            if (!message.Response.Success)
                throw new Exception(message.Response.ErrorMessage);

            return true;
        }

        [HttpGet("Continue")]
        public bool Continue()
        {
            var message = new PlayerMessage(this, PlayerMessage.PlayerControl.Continue);
            _hub.Publish(message);

            if (!message.Response.Success)
                throw new Exception(message.Response.ErrorMessage);

            return true;
        }
    }
}