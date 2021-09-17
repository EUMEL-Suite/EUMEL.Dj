using Eumel.Dj.Core.Models;

namespace Eumel.Dj.Core.Messages
{
    public class GetPlaylistMessage : MessageRequest<DjPlaylist>
    {
        public GetPlaylistMessage(object sender) : base(sender) { }
    }
}