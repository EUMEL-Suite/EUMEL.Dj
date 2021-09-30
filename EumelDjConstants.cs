namespace Eumel.Dj
{
    public class Constants
    {
        public enum EumelLogLevel
        {
            Verbose = 0,
            Debug = 1,
            Information = 2,
            Warn = 3,
            Error = 4,
            Fatal = 5
        }

        public const string SystemChatName = "Eumel";
        public const string ApplicationTitle = "EUMEL Dj";
        public const string ApplicationName = "Eumel.Dj";
        public const string UserToken = "usertoken";
        public const string InvalidTokenException = "InvalidTokenException";
        public const string UnauthorizedEumelException = "UnauthorizedEumelException";
        public const int RequiredSearchLimit = 500; // if songs exceed this limit, a search on the client is required

        public const string EumelSuiteAppData = @"%localappdata%\Eumel Suite";

        // naming conventions for Hub messages:
        //
        // VerbAction => Send messages to bus. Verb written in present and written as active voice. Sample: SendMessage
        // ActionVerb => Received from bus. Verb written in simple past and written as passive voice. Sample: MessageSent
        //
        public class ChatHub
        {
            public const string Route = "chatHub";
            public const string ChatSent = "ChatSent";
            public const string SendChat = "SendChat";
        }
        public class PlaylistHub
        {
            public const string Route = "playlistHub";
            public const string PlaylistChanged = "PlaylistChanged";
        }
        public class Swagger
        {
            public const string JsonEndpoint = "/swagger/v1/swagger.json";
        }
    }
}