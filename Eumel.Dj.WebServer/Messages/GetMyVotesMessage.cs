using System.Collections.Generic;
using Eumel.Dj.WebServer.Models;

namespace Eumel.Dj.WebServer.Messages
{
    public class GetMyVotesMessage : MessageRequest<IEnumerable<Song>>
    {
        public GetMyVotesMessage(object sender) : base(sender) { }
    }
}