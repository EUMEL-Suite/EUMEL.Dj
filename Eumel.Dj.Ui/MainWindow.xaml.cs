using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Windows;
using Eumel.Dj.Ui.Services;
using Eumel.Dj.WebServer;
using Eumel.Dj.WebServer.Messages;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TinyMessenger;

namespace Eumel.Dj.Ui
{
    public partial class MainWindow : Window
    {
        private readonly DjService _djService;
        private readonly TinyMessengerHub _hub;
        private readonly List<TinyMessageSubscriptionToken> _tinyMessageSubscriptions;
        private IWebHost _host;

        public MainWindow()
        {
            InitializeComponent();

            _hub = TinyMessengerHub.DefaultHub;


            _tinyMessageSubscriptions = new List<TinyMessageSubscriptionToken>(new[]
            {
                _hub.Subscribe((Action<ITinyMessage>)LogAllActions)
            });

            _djService = new DjService(_hub, new ItunesProviderService(Settings.Default, _hub));
        }

        private void LogAllActions(ITinyMessage message)
        {
            Dispatcher.Invoke(() =>
            {
                Log.Text = message switch
                {
                    VoteMessage vote =>
                        @$"[Vote] ""{vote.VotersName}"" voted the song ""{vote.Song.Name}"" {vote.Direction.ToString().ToLower()}{Environment.NewLine}{Log.Text}",
                    GetMyVotesMessage getMyVotes =>
                        @$"[Get Votes] ""{getMyVotes.VotersName}"" requested his songs{Environment.NewLine}{Log.Text}",
                    PlayerMessage player =>
                        @$"[Player] Player was requested to {player.PlayerAction.ToString().ToLower()} {player.SongId}{Environment.NewLine}{Log.Text}",
                    LogMessage log => $@"[{log.Level}] {log.Message}",
                    _ => $"[Bus] {message.GetType().Name}{Environment.NewLine}{Log.Text}"
                };
            });
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            if (_host != null) return;

            try
            {
                _host = new WebHostBuilder()
                    .UseKestrel(options =>
                    {
                        options.Listen(IPAddress.Any, 443, listenOptions => { listenOptions.UseHttps(); });
                    })
                    .UseContentRoot(Directory.GetCurrentDirectory())
                    .UseIISIntegration()
                    .UseStartup<Startup>()
                    // this needs to be replaces with a "real DI container" which is provided by this app
                    .ConfigureServices((context, services) => { services.AddSingleton<ITinyMessengerHub>(_hub); })
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

        protected override void OnClosing(CancelEventArgs e)
        {
            _djService.Dispose();
            _tinyMessageSubscriptions.ForEach(x => _hub.Unsubscribe(x));
            _host.StopAsync();

            base.OnClosing(e);
            _host.WaitForShutdown();
        }
    }
}