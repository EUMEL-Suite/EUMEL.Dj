using System;
using System.Collections.Generic;
using Eumel.Dj.WebServer.Messages;
using Eumel.Dj.WebServer.Models;
using Microsoft.AspNetCore.Mvc;
using TinyMessenger;

namespace Eumel.Dj.WebServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DjController : ControllerBase
    {
        private readonly ITinyMessengerHub _hub;

        public DjController(ITinyMessengerHub hub)
        {
            _hub = hub ?? throw new ArgumentNullException(nameof(hub));
        }

        // GET: api/<SongController>
        [HttpGet("GetPlaylist")]
        public DjPlaylist GetPlaylist()
        {
            var plm = new GetPlaylistMessage(this);
            _hub.Publish(plm);
            return plm.Response.Response;
        }

        // GET: api/<SongController>
        [HttpPost("UpVote")]
        public void UpVote([FromBody] Song song, string votersName)
        {
            var plm = new VoteMessage(this, VoteMessage.UpDownVote.Up, song, votersName);
            _hub.Publish(plm);
        }

        // GET: api/<SongController>
        [HttpPost("DownVote")]
        public void DownVote([FromBody] Song song, string votersName)
        {
            var plm = new VoteMessage(this, VoteMessage.UpDownVote.Down, song, votersName);
            _hub.Publish(plm);
        }

        // GET: api/<SongController>
        [HttpGet("GetMyVotes")]
        public IEnumerable<Song> GetMyVotes(string votersName)
        {
            var plm = new GetMyVotesMessage(this, votersName);
            _hub.Publish(plm);
            return plm.Response.Response;
        }
    }
}