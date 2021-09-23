using System;
using System.Collections.Generic;
using Eumel.Dj.Core.Messages;
using Eumel.Dj.Core.Models;
using Microsoft.AspNetCore.Mvc;
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

        [HttpGet("GetChatHistory")]
        public IEnumerable<ChatEntry> GetChatHistory()
        {
            if (string.IsNullOrEmpty(Username))
                return new[] { new ChatEntry { Username = Constants.SystemChatName, Message = "Please login to see chat history!" } };

            var plm = new GetChatHistoryMessage(this);
            _hub.Publish(plm);
            return plm.Response?.Response ?? new[] { new ChatEntry { Username = Constants.SystemChatName, Message = "Chat History not Available" } };
        }
    }
}