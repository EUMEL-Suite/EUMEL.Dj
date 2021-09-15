using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Windows;
using Eumel.Dj.Ui.Services;
using Eumel.Dj.WebServer;
using Eumel.Dj.WebServer.Controllers;
using Eumel.Dj.WebServer.Hubs;
using Eumel.Dj.WebServer.Messages;
using Eumel.Dj.WebServer.Models;
using Eumel.Dj.WebServer.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
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
        private readonly IAppSettings _appSettings;

        public MainWindow()
        {
            InitializeComponent();

            _hub = TinyMessengerHub.DefaultHub;
            _appSettings = new AppSettings()
            {
                RestEndpoint = "https://192.168.178.37:443",
                SyslogServer = "192.168.178.37",
                MinimumLogLevel = "Debug"
            };

            _tinyMessageSubscriptions = new List<TinyMessageSubscriptionToken>(new[]
            {
                _hub.Subscribe((Action<ITinyMessage>)LogAllActions),
                _hub.Subscribe((Action<RequestUserIsAdminMessage>)RequestUserIsAdmin),
                _hub.Subscribe((Action<UserAddedMessage>)UserAdded),
                _hub.Subscribe((Action<UserAddedMessage>)UserRemoved),
                _hub.Subscribe((Action<RequestUserIsAdminMessage>)RequestUserIsAdmin)
            });

            _djService = new DjService(
                _hub,
                new ItunesProviderService(Settings.Default, _hub));

            Loaded += (sender, args) => StartService();
        }


        private void UserRemoved(UserAddedMessage message)
        {
            _hub.Publish(new LogMessage(this, $"User removed from system: {message.Username}", LogLevel.Information));
        }

        private void UserAdded(UserAddedMessage message)
        {
            _hub.Publish(new LogMessage(this, $"User added to system: {message.Username}", LogLevel.Information));
        }

        private void RequestUserIsAdmin(RequestUserIsAdminMessage message)
        {
            message.Response = new MessageResponse<bool>(true);
        }

        private void LogAllActions(ITinyMessage message)
        {
            Dispatcher.Invoke(() =>
            {
                Log.Text = message switch
                {
                    VoteMessage vote => @$"[Vote] ""{vote.VotersName}"" voted the song ""{vote.Song.Name}"" {vote.Direction.ToString().ToLower()}{Environment.NewLine}{Log.Text}",
                    GetMyVotesMessage getMyVotes => @$"[Get Votes] ""{getMyVotes.VotersName}"" requested his songs{Environment.NewLine}{Log.Text}",
                    PlayerMessage player => @$"[Player] Player was requested to {player.PlayerAction.ToString().ToLower()}{Environment.NewLine}{Log.Text}",
                    LogMessage log => $@"[{log.Level}] {log.Message}{Environment.NewLine}{Log.Text}",
                    ChatReceivedMessage chatReceived => $@"{chatReceived.Username}: {chatReceived.Message}{Environment.NewLine}{Log.Text}",
                    ChatSendingMessage chatSent => $@"Request to sent chat {chatSent.Message} by {chatSent.Username}{Environment.NewLine}{Log.Text}",
                    _ => $"[Bus] {message.GetType().Name}{Environment.NewLine}{Log.Text}"
                };
            });
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            StartService();
        }

        private void StartService()
        {
            if (_host != null) return;

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
                        services.AddSingleton<ITinyMessengerHub>(_hub);
                        services.AddSingleton<IAppSettings>(_appSettings);
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

        protected override void OnClosing(CancelEventArgs e)
        {
            _djService.Dispose();
            _tinyMessageSubscriptions.ForEach(x => _hub.Unsubscribe(x));
            _host?.StopAsync();

            base.OnClosing(e);
            _host?.WaitForShutdown();
        }

        private string GetEnvironment()
        {
            return "DEVELOPMENT";
        }

        private void PlayClicked(object sender, RoutedEventArgs e)
        {
            _hub.Publish(new PlayerMessage(this, PlayerMessage.PlayerControl.Play));
        }

        private void NextClicked(object sender, RoutedEventArgs e)
        {
            _hub.Publish(new PlayerMessage(this, PlayerMessage.PlayerControl.Next));
        }

        private void PauseClicked(object sender, RoutedEventArgs e)
        {
            _hub.Publish(new PlayerMessage(this, PlayerMessage.PlayerControl.Pause));
        }

        private void SendChatMessage(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(ChatMessage.Text)) return;
            _hub.Publish(new ChatSendingMessage(this, "[Server]", ChatMessage.Text));
            ChatMessage.Text = string.Empty;
        }
    }
}