namespace Eumel.Dj.WebServer.Messages
{
    public class MessageResponse
    {
        public MessageResponse(string errorMessage = null)
        {
            Success = string.IsNullOrWhiteSpace(errorMessage);
            ErrorMessage = Success ? null : errorMessage;
        }

        public bool Success { get; set; }

        public string ErrorMessage { get; set; }
    }

    public class MessageResponse<TResponse> : MessageResponse
    {
        public MessageResponse(TResponse response, string errorMessage = null)
            : base(errorMessage)
        {
            Response = response;
        }

        public TResponse Response { get; set; }
    }
}