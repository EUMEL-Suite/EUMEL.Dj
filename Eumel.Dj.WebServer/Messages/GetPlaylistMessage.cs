using Eumel.Dj.WebServer.Controllers;

namespace Eumel.Dj.WebServer.Messages
{
    public class GetPlaylistMessage : MessageRequest<DjPlaylist>
    {
        public GetPlaylistMessage(object sender) : base(sender) { }
    }
}