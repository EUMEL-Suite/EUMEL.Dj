using System;
using System.Net.Http;
using System.Threading.Tasks;
using Eumel.Dj.WebServer.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Client;
using TinyMessenger;

namespace Eumel.Dj.WebServer.Hubs
{
    public class ChatAdapterService : AdapterServiceBase
    {
        private readonly HubConnection _connection;

        public ChatAdapterService(IHubContext<PlaylistHub> clientHub, ITinyMessengerHub serverHub, IAppSettings settings) : base(clientHub, serverHub)
        {
            Subscribe((Action<ChatSentMessage>)(async (x) => await SendChatSentAsync(x.Username, x.Message)));

            _connection = new HubConnectionBuilder()
                .WithUrl($"{settings.RestEndpoint}/{ChatHub.Route}", options =>
                {
                    options.Headers.Add("usertoken", Guid.NewGuid().ToString());
                    options.HttpMessageHandlerFactory = message =>
                    {
                        if (message is HttpClientHandler clientHandler)
                            // always verify the SSL certificate
                            clientHandler.ServerCertificateCustomValidationCallback +=
                                (sender, certificate, chain, sslPolicyErrors) => true;
                        return message;
                    };
                })
                .Build();

            _connection.Closed += async (error) =>
            {
                await Task.Delay(new Random().Next(0, 5) * 1000);
                await _connection.StartAsync();
            };
            _connection.StartAsync();

            _connection.On<string, string>(ChatHub.ChatSent, (username, message) =>
            {
                serverHub.Publish(new ChatReceivedMessage(this, username, message));
            });
        }

        private async Task SendChatSentAsync(string username, string message)
        {
            await ClientHub.Clients.All.SendAsync(nameof(ChatHub.SendChatMessage), username, message);
        }
    }
}