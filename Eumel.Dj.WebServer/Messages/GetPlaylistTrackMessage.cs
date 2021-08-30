using System.Collections.Generic;
using Eumel.Dj.WebServer.Controllers;

namespace Eumel.Dj.WebServer.Messages
{
    public class GetPlaylistTrackMessage : MessageRequest<IEnumerable<EumelTrack>>
    {
        public string PlaylistName { get; }

        public GetPlaylistTrackMessage(object sender, string playlistName)
            : base(sender)
        {
            PlaylistName = playlistName;
        }
    }
}