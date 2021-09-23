using System;
using System.Collections.Generic;
using System.Linq;
using Eumel.Dj.Core.Messages;
using Eumel.Dj.Core.Models;
using TinyMessenger;

namespace Eumel.Dj.Ui.AutoStartServices
{
    public class ChatCacheService : IAutoStart
    {
        private readonly List<TinyMessageSubscriptionToken> _tinyMessageSubscriptions = new();
        private readonly ITinyMessengerHub _hub;
        private readonly IList<ChatEntry> _chats = new List<ChatEntry>();

        public ChatCacheService(ITinyMessengerHub hub)
        {
            _hub = hub ?? throw new ArgumentNullException(nameof(hub));
        }
        public void Start()
        {
            _tinyMessageSubscriptions.AddRange(new List<TinyMessageSubscriptionToken>(new[]
            {
                _hub.Subscribe((Action<ChatReceivedMessage>)ChatReceived),
                _hub.Subscribe((Action<GetChatHistoryMessage>)GetChatHistory)
            }));

        }

        public void Stop()
        {
            _tinyMessageSubscriptions.ForEach(x => _hub.Unsubscribe(x));
        }

        private void GetChatHistory(GetChatHistoryMessage message)
        {
            message.Response = new MessageResponse<IEnumerable<ChatEntry>>(_chats.ToArray());
        }

        private void ChatReceived(ChatReceivedMessage message)
        {
            _chats.Add(new ChatEntry() { Username = message.Username, Message = message.Message });
        }
    }
}