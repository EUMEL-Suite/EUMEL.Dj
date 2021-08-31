using System.Collections.Generic;
using Eumel.Dj.WebServer.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Eumel.Dj.WebServer.Messages
{
    public class GetSongsMessage : MessageRequest<IEnumerable<Song>>
    {
        public int Skip { get; }
        public int Take { get; }

        public GetSongsMessage(object sender, int skip, int take):base(sender)
        {
            Skip = skip;
            Take = take;
        }
    }
}
