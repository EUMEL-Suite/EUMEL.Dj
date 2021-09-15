using System;
using System.ComponentModel;
using Eumel.Dj.WebServer.Exceptions;
using Eumel.Dj.WebServer.Messages;
using Eumel.Dj.WebServer.Models;
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

        public PlayerController(ITinyMessengerHub hub, ITokenService tokenService) : base(tokenService)
        {
            _hub = hub ?? throw new ArgumentNullException(nameof(hub));
        }
        
        [HttpGet("Play")]
        [Description("Starts the player. In case the media is empty, a new song will be loaded.")]
        public bool Play()
        {
            if (string.IsNullOrEmpty(Username))
                throw new InvalidTokenException("Token seems to be invalid. Please login again");

            ValidateAdminPermissions();

            var message = new PlayerMessage(this, PlayerMessage.PlayerControl.Play);
            _hub.Publish(message);

            if (!message.Response.Success)
                throw new Exception(message.Response.ErrorMessage);

            return true;
        }

        [HttpGet("Stop")]
        [Description("Stops the player and sets the time to 0:00")]
        public bool Stop()
        {
            if (string.IsNullOrEmpty(Username))
                throw new InvalidTokenException("Token seems to be invalid. Please login again");

            ValidateAdminPermissions();

            var message = new PlayerMessage(this, PlayerMessage.PlayerControl.Stop);
            _hub.Publish(message);

            if (!message.Response.Success)
                throw new Exception(message.Response.ErrorMessage);

            return true;
        }

        [HttpGet("Pause")]
        [Description("Pauses the player but the time index will remain the same.")]
        public bool Pause()
        {
            if (string.IsNullOrEmpty(Username))
                throw new InvalidTokenException("Token seems to be invalid. Please login again");

            ValidateAdminPermissions();

            var message = new PlayerMessage(this, PlayerMessage.PlayerControl.Pause);
            _hub.Publish(message);

            if (!message.Response.Success)
                throw new Exception(message.Response.ErrorMessage);

            return true;
        }

        [HttpGet("Next")]
        [Description("Plays the next song from the queue.")]
        public bool Next()
        {
            if (string.IsNullOrEmpty(Username))
                throw new InvalidTokenException("Token seems to be invalid. Please login again");

            ValidateAdminPermissions();

            var message = new PlayerMessage(this, PlayerMessage.PlayerControl.Next);
            _hub.Publish(message);

            return message.Response.Success;
        }

        [HttpGet("Restart")]
        [Description("Restarts the current song.")]
        public bool Restart()
        {
            if (string.IsNullOrEmpty(Username))
                throw new InvalidTokenException("Token seems to be invalid. Please login again");

            ValidateAdminPermissions();

            var message = new PlayerMessage(this, PlayerMessage.PlayerControl.Restart);
            _hub.Publish(message);

            return message.Response.Success;
        }

        /// <summary>
        /// check if the current user has admin permissions
        /// </summary>
        private void ValidateAdminPermissions()
        {
            var adminMessage = new RequestUserIsAdminMessage(this, Username);
            _hub.Publish(adminMessage);
            if (!adminMessage.Response.Response)
                throw new UnauthorizedEumelException($"User {Username} is has no admin permissions.");
        }

        [HttpGet("Status")]
        public PlayerStatus Status()
        {
            var message = new PlayerStatusMessage(this);
            _hub.Publish(message);

            return message.Response.Response;
        }
    }
}