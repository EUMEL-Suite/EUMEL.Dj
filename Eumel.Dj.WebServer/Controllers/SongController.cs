using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using Eumel.Dj.WebServer.Messages;
using Eumel.Dj.WebServer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TinyMessenger;

namespace Eumel.Dj.WebServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SongController : EumelDjControllerBase
    {
        private readonly ITinyMessengerHub _hub;

        public SongController(ITinyMessengerHub hub)
        {
            _hub = hub ?? throw new ArgumentNullException(nameof(hub));
        }

        // GET: api/<SongController>
        [HttpGet("GetSongsSource")]
        public SongsSource GetSongsSource()
        {
            var plm = new GetSongsSourceMessage(this);
            _hub.Publish(plm);
            return plm.Response.Response;
        }

        // GET: api/<SongController>
        [HttpGet("GetSongs")]
        public IEnumerable<Song> GetSongs(int skip, int take)
        {
            var plm = new GetSongsMessage(this, skip, take);
            _hub.Publish(plm);
            return plm.Response.Response;
        }
    }
}