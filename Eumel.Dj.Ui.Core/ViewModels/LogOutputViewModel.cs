using System;
using System.Collections.Generic;
using Caliburn.Micro;
using Eumel.Dj.Core.Messages;
using Eumel.Dj.Ui.Core.Interfaces;
using TinyMessenger;

namespace Eumel.Dj.Ui.Core.ViewModels
{
    public class LogOutputViewModel : PropertyChangedBase, ILogOutputViewModel, IDisposable
    {
        private readonly ITinyMessengerHub _hub;
        private readonly List<TinyMessageSubscriptionToken> _tinyMessageSubscriptions;
        private string _logMessages;

        public LogOutputViewModel(ITinyMessengerHub hub)
        {
            _hub = hub ?? throw new ArgumentNullException(nameof(hub));

            _tinyMessageSubscriptions = new List<TinyMessageSubscriptionToken>(new[]
            {
                _hub.Subscribe((Action<ITinyMessage>)LogAll),
                _hub.Subscribe((Action<LogMessage>)Log)
            });
        }

        public void LogAll(ITinyMessage message)
        {
            LogMessages = LogMessages + message.GetType().Name + Environment.NewLine;
        }

        public void Log(LogMessage message)
        {
            LogMessages = LogMessages + message.Message + Environment.NewLine;
        }

        public string LogMessages
        {
            get => _logMessages;
            set
            {
                _logMessages = value;
                NotifyOfPropertyChange(() => LogMessages);
                NotifyOfPropertyChange(() => CanClear);
            }
        }

        public bool CanClear => !string.IsNullOrWhiteSpace(LogMessages);

        public void Clear()
        {
            LogMessages = string.Empty;
            NotifyOfPropertyChange(() => LogMessages);
        }

        public void Dispose()
        {
            _tinyMessageSubscriptions.ForEach(x => _hub.Unsubscribe(x));
        }
    }
}