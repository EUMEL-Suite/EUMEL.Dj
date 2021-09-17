using Eumel.Dj.Core.Models;

namespace Eumel.Dj.Core.Messages
{
    public class GetSongsSourceMessage : MessageRequest<SongsSource>
    {
        public GetSongsSourceMessage(object sender) : base(sender)
        {
        }
    }
}