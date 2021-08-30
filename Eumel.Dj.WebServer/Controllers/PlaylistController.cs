using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using TinyMessenger;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplication1.Controllers
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
        public IEnumerable<EumelTrack> Get()
        {
            var plm = new PlaylistMessage(this);
            _hub.Publish(plm);

            return plm.Playlist;
        }

        // GET api/<PlaylistController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<PlaylistController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
            _hub.Publish(new SomeMessage(this, value + " " + DateTime.Now.ToShortTimeString()));
        }

        // PUT api/<PlaylistController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<PlaylistController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        // POST api/<PlaylistController>/Play
        [HttpPost("Play")]
        public void Play([FromBody] string value)
        {
            _hub.Publish(new PlaysongMessage(this, value));
        }
    }

    public class PlaysongMessage : ITinyMessage
    {
        public object Sender { get; }
        public string Location { get; }

        public PlaysongMessage(object sender, string location)
        {
            Sender = sender;
            Location = location;
        }
    }

    public class PlaylistMessage : ITinyMessage
    {
        public PlaylistMessage(object sender)
        {
            Sender = sender;
        }

        public object Sender { get; }

        public IEnumerable<EumelTrack> Playlist { get; set; }
    }

    public class SomeMessage : ITinyMessage
    {
        public SomeMessage(object sender, string message)
        {
            Sender = sender;
            Message = message;
        }

        public object Sender { get; }
        public string Message { get; }
    }

    public class EumelTrack
    {
        public string Name { get; set; }
        public string Album { get; set; }
        public string Artist { get; set; }
        public string AlbumArtist { get; set; }
        public string Location { get; set; }
    }
}
