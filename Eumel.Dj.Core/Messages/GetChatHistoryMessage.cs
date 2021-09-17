using System.Collections.Generic;
using Eumel.Dj.Core.Models;

namespace Eumel.Dj.Core.Messages
{
    public class GetChatHistoryMessage : MessageRequest<IEnumerable<ChatEntry>>
    {
        public GetChatHistoryMessage(object sender) : base(sender) { }
    }
}