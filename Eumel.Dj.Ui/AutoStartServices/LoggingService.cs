using System;
using System.Collections.Generic;
using Eumel.Dj.Core.Logging;
using Eumel.Dj.Core.Messages;
using Microsoft.Extensions.Logging;
using TinyMessenger;

namespace Eumel.Dj.Ui.AutoStartServices
{
    public class LoggingService : IAutoStart
    {
        private readonly ITinyMessengerHub _hub;
        private readonly IEumelLogger _logger;
        private readonly List<TinyMessageSubscriptionToken> _tinyMessageSubscriptions = new();

        public LoggingService(ITinyMessengerHub hub, IEumelLogger logger)
        {
            _hub = hub ?? throw new ArgumentNullException(nameof(hub));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        }

        private void Log(LogMessage message)
        {
            switch (message.Level)
            {
                case LogLevel.Trace:
                    _logger.Verbose(message.Message);
                    break;
                case LogLevel.Debug:
                    _logger.Debug(message.Message);
                    break;
                case LogLevel.Information:
                    _logger.Information(message.Message);
                    break;
                case LogLevel.Warning:
                    _logger.Warning(message.Message);
                    break;
                case LogLevel.Error:
                    _logger.Error(message.Message, message.Exception);
                    break;
                case LogLevel.Critical:
                    _logger.Fatal(message.Message, message.Exception);
                    break;
                case LogLevel.None:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void Start()
        {
            _tinyMessageSubscriptions.AddRange(new List<TinyMessageSubscriptionToken>(new[]
            {
                _hub.Subscribe((Action<LogMessage>)Log),
            }));
        }

        public void Stop()
        {
            _tinyMessageSubscriptions.ForEach(x => _hub.Unsubscribe(x));
        }
    }
}