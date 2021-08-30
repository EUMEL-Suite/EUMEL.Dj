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
        public bool Play(string location)
        {
            var message = new PlayerMessageRequest(this, PlayerMessageRequest.PlayerControl.Play, location);
            _hub.Publish(message);

            if (!message.Response.Success)
                throw new Exception(message.Response.ErrorMessage);

            return true;
        }

        [HttpGet("Pause")]
        public bool Pause()
        {
            var message = new PlayerMessageRequest(this, PlayerMessageRequest.PlayerControl.Pause);
            _hub.Publish(message);

            if (!message.Response.Success)
                throw new Exception(message.Response.ErrorMessage);

            return true;
        }

        [HttpGet("Continue")]
        public bool Continue()
        {
            var message = new PlayerMessageRequest(this, PlayerMessageRequest.PlayerControl.Continue);
            _hub.Publish(message);

            if (!message.Response.Success)
                throw new Exception(message.Response.ErrorMessage);

            return true;
        }
    }
}