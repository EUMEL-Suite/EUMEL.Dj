using Eumel.Dj.WebServer.Messages;

namespace Eumel.Dj.WebServer.Controllers
{
    public class PlayerStatusMessage : MessageRequest<PlayerMessage.PlayerControl>
    {
        public PlayerStatusMessage(object sender) : base(sender) { }
    }
}