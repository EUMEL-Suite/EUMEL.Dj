using System;
using Caliburn.Micro;
using Eumel.Dj.Core.Messages;
using Eumel.Dj.Ui.Core.Interfaces;
using TinyMessenger;

namespace Eumel.Dj.Ui.Core.ViewModels
{
    public class ChatViewModel : Screen, IChatViewModel
    {
        private readonly ITinyMessengerHub _hub;
        private string _chatHistory;
        private string _chatMessage;

        public string ChatHistory
        {
            get => _chatHistory;
            set
            {
                if (value == _chatHistory) return;
                _chatHistory = value;
                NotifyOfPropertyChange();
            }
        }

        public string ChatMessage
        {
            get => _chatMessage;
            set
            {
                if (value == _chatMessage) return;
                _chatMessage = value;
                NotifyOfPropertyChange();
            }
        }

        public void SendMessage()
        {
            _hub.Publish(new ChatSendingMessage(this, Constants.SystemChatName, ChatMessage));
        }

        public ChatViewModel(ITinyMessengerHub hub)
        {
            _hub = hub ?? throw new ArgumentNullException(nameof(hub));

            _ = _hub.Subscribe((Action<ChatReceivedMessage>)ChatReceived);
        }



        private void ChatReceived(ChatReceivedMessage message)
        {
            ChatHistory = ChatHistory + Environment.NewLine + message.Username + ": " + message.Message;
        }
    }
}