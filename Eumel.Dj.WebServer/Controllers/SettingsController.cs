using System;
using System.Drawing.Imaging;
using System.IO;
using Eumel.Dj.WebServer.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using QRCoder;
using TinyMessenger;

namespace Eumel.Dj.WebServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SettingsController : ControllerBase
    {
        private readonly ITinyMessengerHub _hub;
        private readonly IAppSettings _settings;

        public SettingsController(ITinyMessengerHub hub, IAppSettings appSettings)
        {
            _hub = hub ?? throw new ArgumentNullException(nameof(hub));
            _settings = appSettings ?? throw new ArgumentNullException(nameof(appSettings));
        }

        [HttpGet("Init")]
        public FileResult Init()
        {
            var qrCodeGenerator = new QRCodeGenerator();
            var qrCodeData = qrCodeGenerator.CreateQrCode(new PayloadGenerator.Url(_settings.RestEndpoint), QRCodeGenerator.ECCLevel.Q);
            var qrCode = new QRCode(qrCodeData);

            var qrCodeImage = qrCode.GetGraphic(5);
            var ms = new MemoryStream();
            qrCodeImage.Save(ms, ImageFormat.Jpeg);
            ms.Position = 0;
            return File(ms, "image/jpeg");
        }

        [HttpGet("CheckUsername")]
        public bool CheckUsername(string username)
        {
            // todo implement me
            return true;
        }

        [HttpGet("RequestSettingsAndToken")]
        public ServerSettings RequestSettingsAndToken(string username)
        {
            var message = new RequestUserTokenMessage(this, username);
            _hub.Publish(message);
            if (message.Response == null)
                throw new EumelDjException("A user token cannot be created");

            var settings = new ServerSettings()
            {
                Username = message.Response.Response.Username,
                Usertoken = message.Response.Response.Usertoken,
                SyslogServer = _settings.SyslogServer,

                MinimumLogLevel = _settings.MinimumLogLevel
            };

            return settings;
        }
    }
}