using System;
using System.Drawing.Imaging;
using System.IO;
using Eumel.Dj.Core.Exceptions;
using Eumel.Dj.Core.Messages;
using Eumel.Dj.Core.Models;
using Microsoft.AspNetCore.Mvc;
using TinyMessenger;

namespace Eumel.Dj.WebServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SettingsController : EumelDjControllerBase
    {
        private readonly ITinyMessengerHub _hub;
        private readonly IAppSettings _settings;
        private readonly ITokenService _tokenService;
        private readonly IQrCodeService _qrCodeService;

        public SettingsController(
            ITinyMessengerHub hub,
            IAppSettings appSettings,
            IQrCodeService qrCodeService,
            ITokenService tokenService) : base(tokenService)
        {
            _hub = hub ?? throw new ArgumentNullException(nameof(hub));
            _settings = appSettings ?? throw new ArgumentNullException(nameof(appSettings));
            _tokenService = tokenService ?? throw new ArgumentNullException(nameof(tokenService));
            _qrCodeService = qrCodeService ?? throw new ArgumentNullException(nameof(qrCodeService));
        }

        [HttpGet("Init")]
        public FileResult Init()
        {
            var ms = new MemoryStream();
            _qrCodeService
                .CreateInitCode(_settings.RestEndpoint)
                .Save(ms, ImageFormat.Jpeg);

            ms.Position = 0;
            return File(ms, "image/jpeg");
        }

        [HttpGet("CheckUserIsAdmin")]
        public bool CheckUserIsAdmin()
        {
            var message = new RequestUserIsAdminMessage(this, Username);
            _hub.Publish(message);

            if (message.Response == null)
                throw new EumelDjException("None returned a proper answer. Cannot verify if user is an admin");

            return message.Response.Response;
        }

        [HttpGet("TokenIsValid")]
        public bool TokenIsValid()
        {
            return _tokenService.TryFindUser(Token, out _);
        }

        [HttpGet("CheckUsername")]
        public bool CheckUsername(string username)
        {
            return _tokenService.UsernameIsAvailable(username, out _);
        }

        [HttpGet("RequestSettingsAndToken")]
        public ServerSettings RequestSettingsAndToken(string username)
        {
            var userToken = _tokenService.GetUserToken(username);

            var settings = new ServerSettings()
            {
                Username = userToken.Username,
                Usertoken = userToken.Usertoken,

                SyslogServer = _settings.SyslogServer,
                MinimumLogLevel = _settings.MinimumLogLevel
            };

            _hub.Publish(new UserAddedMessage(this, settings.Username));

            return settings;
        }

        [HttpGet("Logout")]
        public void Logout()
        {
            if (!string.IsNullOrWhiteSpace(Username))
            {
                _hub.Publish(new ClearMyVotesMessage(this, Username));
                _hub.Publish(new UserRemovedMessage(this, Username));
            }
            _tokenService.DisposeUserToken(Token);
        }
    }
}