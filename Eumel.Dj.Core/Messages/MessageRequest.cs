using System.Transactions;
using TinyMessenger;

namespace Eumel.Dj.Core.Messages
{
    public abstract class MessageRequest : ITinyMessage
    {
        protected MessageRequest(object sender)
        {
            Sender = sender;
        }

        public object Sender { get; }
    }

    public class ServiceStatusChangedMessage : MessageRequest
    {
        public ServiceStatus Status { get; }

        public ServiceStatusChangedMessage(object sender, ServiceStatus serviceStatus) : base(sender)
        {
            Status = serviceStatus;
        }

        public enum ServiceStatus
        {
            Started,
            Stopped
        }
    }

    public abstract class MessageRequest<TResponse> : ITinyMessage
    {
        protected MessageRequest(object sender)
        {
            Sender = sender;
        }

        public MessageResponse<TResponse> Response { get; set; }

        public object Sender { get; }
    }
}