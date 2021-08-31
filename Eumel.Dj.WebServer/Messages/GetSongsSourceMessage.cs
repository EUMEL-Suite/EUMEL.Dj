using Eumel.Dj.WebServer.Models;

namespace Eumel.Dj.WebServer.Messages
{
    public class GetSongsSourceMessage : MessageRequest<SongsSource>
    {
        public GetSongsSourceMessage(object sender) : base(sender) { }
    }
}