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
        [HttpGet("UpVote")]
        public bool UpVote(Song song)
        {
            var plm = new VoteMessage(this, VoteMessage.UpDownVote.Up, song);
            _hub.Publish(plm);
            return plm.Response.Response;
        }

        // GET: api/<SongController>
        [HttpGet("DownVote")]
        public bool DownVote(Song song)
        {
            var plm = new VoteMessage(this, VoteMessage.UpDownVote.Down, song);
            _hub.Publish(plm);
            return plm.Response.Response;
        }

        // GET: api/<SongController>
        [HttpGet("GetMyVotes")]
        public IEnumerable<Song> GetMyVotes()
        {
            var plm = new GetMyVotesMessage(this);
            _hub.Publish(plm);
            return plm.Response.Response;
        }
    }
}