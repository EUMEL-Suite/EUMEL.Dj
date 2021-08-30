using TinyMessenger;

namespace Eumel.Dj.WebServer.Messages
{
    public abstract class MessageRequest : ITinyMessage
    {
        protected MessageRequest(object sender)
        {
            Sender = sender;
        }

        public object Sender { get; }
        public MessageResponse Response { get; set; }
    }

    public abstract class MessageRequest<TResponse> : ITinyMessage
    {
        protected MessageRequest(object sender)
        {
            Sender = sender;
        }

        public object Sender { get; }
        public MessageResponse<TResponse> Response { get; set; }
    }
}