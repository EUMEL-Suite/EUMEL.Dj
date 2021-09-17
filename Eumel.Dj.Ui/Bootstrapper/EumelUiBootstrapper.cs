using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Windows;
using Caliburn.Micro;
using Eumel.Dj.Core.Messages;
using Eumel.Dj.Core.Models;
using Eumel.Dj.Ui.Core.ViewModels;
using Eumel.Dj.WebServer;
using Eumel.Dj.WebServer.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using StructureMap;
using TinyMessenger;

namespace Eumel.Dj.Ui.Bootstrapper
{
    public class EumelUiBootstrapper : BootstrapperBase
    {
        private Container _container;
        private IWebServiceHost _hostService;

        public EumelUiBootstrapper()
        {
            Initialize();
        }

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            DisplayRootViewFor<ShellViewModel>();

            _hostService = (IWebServiceHost)GetInstance(typeof(IWebServiceHost), null);
            _hostService.Start();
        }

        protected override IEnumerable<Assembly> SelectAssemblies()
        {
            return base.SelectAssemblies()
                .Append(Assembly.GetAssembly(typeof(ShellViewModel)));
        }

        protected override void Configure()
        {
            _container = new Container(_ =>
            {
                _.AddRegistry<CoreServicesRegistry>();
                _.AddRegistry<ViewModelsRegistry>();
            });
        }

        protected override object GetInstance(Type service, string key)
        {
            return key == null
                ? _container.GetInstance(service)
                : _container.GetInstance(service, key);
        }

        protected override IEnumerable<object> GetAllInstances(Type service)
        {
            return _container.GetAllInstances(service).Cast<object>();
        }
    }

    public interface IWebServiceHost
    {
        void Start();
    }

    public class WebServiceHost : IWebServiceHost
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
                _host.RunAsync();
                _hub.Publish(new LogMessage(this, "Service started at *:443", LogLevel.Information));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                _hub.Publish(new LogMessage(this, ex.Message, LogLevel.Error));
            }
        }
        private string GetEnvironment()
        {
            return "DEVELOPMENT";
        }

    }
}
