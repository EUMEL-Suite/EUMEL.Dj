using System;
using System.Collections.Generic;
using Eumel.Dj.WebServer.Models;

namespace Eumel.Dj.WebServer.Services
{
    public class TokenService : ITokenService
    {
        private readonly Dictionary<string, string> _tokenToUserDictionary = new();

        public UserToken GetUserToken(string usernameRequest)
        {
            _ = UsernameIsAvailable(usernameRequest, out var username);
            var token = Guid.NewGuid().ToString();

            _tokenToUserDictionary.Add(token, username);

            return new UserToken(username, token);
        }

        public bool UsernameIsAvailable(string usernameRequest, out string usernameRecommendation)
        {
            // if user not taken, use it :-)
            if (_tokenToUserDictionary.ContainsValue(usernameRequest))
            {
                usernameRecommendation = usernameRequest;
                return true;
            }

            // lets append a random number to the username
            var counter = 1;
            usernameRecommendation = usernameRequest + counter;
            while (_tokenToUserDictionary.ContainsValue(usernameRecommendation))
                usernameRecommendation = usernameRequest + ++counter;

            return false;
        }

        public bool TryFindUser(string token, out string username)
        {
            username = string.Empty;
            if (string.IsNullOrWhiteSpace(token) || !_tokenToUserDictionary.ContainsKey(token))
                return false;

            username = _tokenToUserDictionary[token];
            return true;
        }
    }
}