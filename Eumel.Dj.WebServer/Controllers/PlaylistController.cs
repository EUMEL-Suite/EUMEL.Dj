using System;
using System.Collections.Generic;
using Eumel.Dj.WebServer.Messages;
using Microsoft.AspNetCore.Mvc;
using TinyMessenger;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Eumel.Dj.WebServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlaylistController : ControllerBase
    {
        private readonly ITinyMessengerHub _hub;

        public PlaylistController(ITinyMessengerHub hub)
        {
            _hub = hub ?? throw new ArgumentNullException(nameof(hub));
        }

        // GET: api/<PlaylistController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            var plm = new GetPlaylistMessage(this);
            _hub.Publish(plm);
            return plm.Response.Response;
        }

        // GET api/<PlaylistController>/5
        [HttpGet("{name}")]
        public IEnumerable<EumelTrack> Get(string name)
        {
            var plm = new GetPlaylistTrackMessage(this, name);
            _hub.Publish(plm);
            return plm.Response.Response;
        }
    }
}
