using System.Collections.Generic;

namespace Eumel.Dj.WebServer.Messages
{
    public class GetPlaylistMessage : MessageRequest<IEnumerable<string>>
    {
        public GetPlaylistMessage(object sender)
            : base(sender)
        {
        }
    }
}