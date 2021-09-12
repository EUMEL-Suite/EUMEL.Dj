using Eumel.Dj.WebServer.Models;

namespace Eumel.Dj.WebServer.Messages
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