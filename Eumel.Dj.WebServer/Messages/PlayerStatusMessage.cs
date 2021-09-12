using Eumel.Dj.WebServer.Controllers;

namespace Eumel.Dj.WebServer.Messages
{
    public class PlayerStatusMessage : MessageRequest<PlayerStatus>
    {
        public PlayerStatusMessage(object sender) : base(sender) { }
    }
}