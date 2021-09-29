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
    public class SongController : EumelDjControllerBase
    {
        private readonly ITinyMessengerHub _hub;

        public SongController(ITinyMessengerHub hub, ITokenService tokenService) : base(tokenService)
        {
            _hub = hub ?? throw new ArgumentNullException(nameof(hub));
        }

        [HttpGet("GetSongsSource")]
        public SongsSource GetSongsSource()
        {
            var plm = new GetSongsSourceMessage(this);
            _hub.Publish(plm);
            return plm.Response.Response;
        }

        [HttpGet("GetSongs")]
        public IEnumerable<Song> GetSongs(int skip, int take)
        {
            var plm = new GetSongsMessage(this, skip, take);
            _hub.Publish(plm);
            return plm.Response.Response;
        }

        [HttpGet("SearchSongs")]
        public IEnumerable<Song> SearchSongs(string searchText)
        {
            var plm = new SearchSongsMessage(this, searchText);
            _hub.Publish(plm);
            return plm.Response.Response;
        }
    }
}