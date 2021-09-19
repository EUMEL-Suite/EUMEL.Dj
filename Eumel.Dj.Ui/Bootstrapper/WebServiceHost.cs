using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using Eumel.Dj.Core.Messages;
using Eumel.Dj.Core.Models;
using Eumel.Dj.WebServer;
using Eumel.Dj.WebServer.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TinyMessenger;

namespace Eumel.Dj.Ui.Bootstrapper
{
    public class WebServiceHost : IWebServiceHost, IDisposable
    {
        private readonly ITinyMessengerHub _hub;
        private readonly IAppSettings _settings;
        private IWebHost _host;

        public WebServiceHost(ITinyMessengerHub hub, IAppSettings settings)
        {
            _hub = hub ?? throw new ArgumentNullException(nameof(hub));
            _settings = settings ?? throw new ArgumentNullException(nameof(settings));
        }
        public void Start()
        {
            try
            {
                _host = new WebHostBuilder()
                    .UseEnvironment(GetEnvironment())
                    .UseKestrel(options =>
                    {
                        options.Listen(IPAddress.Any, 443, listenOptions => { listenOptions.UseHttps(); });
                    })
                    .UseContentRoot(Directory.GetCurrentDirectory())
                    .UseIISIntegration()
                    .UseStartup<Startup>()
                    // this needs to be replaces with a "real DI container" which is provided by this app
                    .ConfigureServices((context, services) =>
                    {
                        services.AddSingleton(_hub);
                        services.AddSingleton(_settings);
                        services.AddSingleton<IQrCodeService>(new QrCodeService());
                        services.AddSingleton<ITokenService>(new TokenService());
                    })
                    .Build();
                _host.Start();

                _hub.Publish(new LogMessage(this, "Service started at *:443", LogLevel.Information));
                _hub.Publish(new ServiceStatusChangedMessage(this, ServiceStatusChangedMessage.ServiceStatus.Started));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                _hub.PublishAsync(new LogMessage(this, ex.Message, LogLevel.Error));
            }
        }

        public async void Stop()
        {
            await _host.StopAsync();
            await _host.WaitForShutdownAsync();
            _hub.PublishAsync(new ServiceStatusChangedMessage(this, ServiceStatusChangedMessage.ServiceStatus.Stopped));
        }

        private string GetEnvironment()
        {
            return "DEVELOPMENT";
        }

        public void Dispose()
        {
            _host?.Dispose();
        }
    }
}