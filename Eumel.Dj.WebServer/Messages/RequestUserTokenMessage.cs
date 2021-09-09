using Eumel.Dj.WebServer.Messages;

namespace Eumel.Dj.WebServer.Controllers
{
    public class RequestUserTokenMessage : MessageRequest<UserToken>
    {
        public string UsernameRequest { get; }

        public RequestUserTokenMessage(object sender, string usernameRequest) : base(sender)
        {
            UsernameRequest = usernameRequest;
        }
    }
}