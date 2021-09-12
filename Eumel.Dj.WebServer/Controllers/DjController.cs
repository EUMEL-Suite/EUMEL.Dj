﻿using System;
using System.Collections.Generic;
using Eumel.Dj.WebServer.Messages;
using Eumel.Dj.WebServer.Models;
using Microsoft.AspNetCore.Mvc;
using TinyMessenger;

namespace Eumel.Dj.WebServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DjController : EumelDjControllerBase
    {
        private readonly ITinyMessengerHub _hub;

        public DjController(ITinyMessengerHub hub, ITokenService tokenService) : base(tokenService)
        {
            _hub = hub ?? throw new ArgumentNullException(nameof(hub));
        }

        [HttpGet("GetPlaylist")]
        public DjPlaylist GetPlaylist()
        {
            var plm = new GetPlaylistMessage(this);
            _hub.Publish(plm);
            return plm.Response.Response;
        }

        [HttpPost("UpVote")]
        public void UpVote([FromBody] Song song)
        {
            var plm = new VoteMessage(this, VoteMessage.UpDownVote.Up, song, Username);
            _hub.Publish(plm);
        }

        [HttpPost("DownVote")]
        public void DownVote([FromBody] Song song)
        {
            var plm = new VoteMessage(this, VoteMessage.UpDownVote.Down, song, Username);
            _hub.Publish(plm);
        }

        [HttpGet("GetMyVotes")]
        public IEnumerable<Song> GetMyVotes()
        {
            var plm = new GetMyVotesMessage(this, Username);
            _hub.Publish(plm);
            return plm.Response.Response;
        }

        [HttpGet("ClearMyVotes")]
        public void ClearMyVotes()
        {
            var plm = new ClearMyVotesMessage(this, Username);
            _hub.Publish(plm);
        }
    }
}