using System.Collections.Generic;
using Eumel.Dj.Core.Models;

namespace Eumel.Dj.Core.Messages
{
    public class GetMyVotesMessage : MessageRequest<IEnumerable<Song>>
    {
        public GetMyVotesMessage(object sender, string votersName) : base(sender)
        {
            VotersName = votersName;
        }
        public string VotersName { get; }
    }
}