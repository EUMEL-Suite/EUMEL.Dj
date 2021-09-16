using System.Collections.Generic;
using Eumel.Dj.WebServer.Controllers;

namespace Eumel.Dj.WebServer.Messages
{
    public class GetChatHistoryMessage : MessageRequest<IEnumerable<ChatEntry>>
    {
        public GetChatHistoryMessage(object sender) : base(sender) { }
    }
}