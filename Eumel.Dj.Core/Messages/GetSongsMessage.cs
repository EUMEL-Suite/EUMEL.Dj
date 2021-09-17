using System.Collections.Generic;
using Eumel.Dj.Core.Models;

namespace Eumel.Dj.Core.Messages
{
    public class GetSongsMessage : MessageRequest<IEnumerable<Song>>
    {
        public GetSongsMessage(object sender, int skip, int take) : base(sender)
        {
            Skip = skip;
            Take = take;
        }

        public int Skip { get; }
        public int Take { get; }
    }
}