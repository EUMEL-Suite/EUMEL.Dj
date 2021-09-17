using Eumel.Dj.Core.Models;

namespace Eumel.Dj.Core.Messages
{
    public class PlaylistChangedMessage : MessageRequest
    {
        public DjPlaylist Playlist { get; }

        public PlaylistChangedMessage(object sender, DjPlaylist playlist) : base(sender)
        {
            Playlist = playlist;
        }
    }
}