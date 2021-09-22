using System;
using System.Collections.Generic;
using Eumel.Dj.Core.Messages;
using Microsoft.Extensions.Logging;
using TinyMessenger;

namespace Eumel.Dj.Ui.AutoStartServices
{
    public class UserService : IAutoStart
    {
        private readonly ITinyMessengerHub _hub;
        private readonly List<TinyMessageSubscriptionToken> _tinyMessageSubscriptions = new();
        private readonly IDictionary<string, bool> _userIsAdminList = new Dictionary<string, bool>();

        public UserService(ITinyMessengerHub hub)
        {
            _hub = hub;

        }

        public void Start()
        {
            _hub.Publish(new LogMessage(this, $"Starting service {GetType().Name} as auto start service", LogLevel.Information));

            _tinyMessageSubscriptions.AddRange(new List<TinyMessageSubscriptionToken>(new[]
            {
                _hub.Subscribe((Action<RequestUserIsAdminMessage>)RequestUserIsAdmin),
                _hub.Subscribe((Action<UserAddedMessage>)UserAdded),
                _hub.Subscribe((Action<UserRemovedMessage>)UserRemoved)
            }));
        }

        private void UserRemoved(UserRemovedMessage message)
        {
            if (_userIsAdminList.ContainsKey(message.Username))
                _userIsAdminList.Remove(message.Username);
        }

        private void UserAdded(UserAddedMessage message)
        {
            if (_userIsAdminList.ContainsKey(message.Username))
            {
                _hub.Publish(new LogMessage(this, "The user already exists in dictionary. This can be a problem with non-unique users. UserService is ignoring second user.", LogLevel.Warning));
                return;
            }

            // for now everyone is an admin. needs to be changed later
            _userIsAdminList.Add(message.Username, true);
        }

        private void RequestUserIsAdmin(RequestUserIsAdminMessage message)
        {
            if (!_userIsAdminList.ContainsKey(message.Username))
            {
                _hub.Publish(new LogMessage(this, $"The user {message.Username} does not exist in dictionary. User must login again. Check user token.", LogLevel.Warning));
                message.Response = new MessageResponse<bool>(false);
                return;
            }

            message.Response = new MessageResponse<bool>(_userIsAdminList[message.Username]);
        }

        public void Stop()
        {
            _tinyMessageSubscriptions.ForEach(x => _hub.Unsubscribe(x));
        }
    }
}