using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Eumel.Dj.Mobile.Services;
using Microsoft.AspNetCore.SignalR.Client;
using Xamarin.Forms;

namespace Eumel.Dj.Mobile.ViewModels
{
    public class ChatViewModel : BaseViewModel
    {
        private string _chat;
        private HubConnection _hub;
        private string _message;
        private string _messagePlaceholder;

        public ChatViewModel()
        {
            SendMessageCommand = new Command(SendMessageAsync);
            LoadChatCommand = new Command(() => TryOrRedirectToLoginAsync(async () =>
            {
                IsBusy = true;
                var chats = await ChatService.GetChatHistory();
                var chatHistory = string.Join(Environment.NewLine, chats.Select(item => item.Username + ": " + item.Message));

                Chat = chatHistory;
                IsBusy = false;
            }, "Loading Chat History"));
        }

        public Command LoadChatCommand { get; }

        public Command SendMessageCommand { get; }

        public string Message
        {
            get => _message;
            set => SetProperty(ref _message, value);
        }

        public string Chat
        {
            get => _chat;
            set => SetProperty(ref _chat, value);
        }

        public string MessagePlaceholder
        {
            get => _messagePlaceholder;
            set => SetProperty(ref _messagePlaceholder, value);
        }

        private async void SendMessageAsync()
        {
            try
            {
                await _hub.InvokeAsync(Constants.ChatHub.SendChat, Settings.Username, Message);
            }
            catch (Exception ex)
            {
                SyslogService.Error(ex.Message);
            }
        }

        public async void OnAppearing()
        {
            LoadChatCommand.Execute(null);
            MessagePlaceholder = $"send as {Settings.Username}...";

            if (string.IsNullOrWhiteSpace(Settings.RestEndpoint))
                return;

            if (_hub != null) return;

            _hub = new HubConnectionBuilder()
                .WithUrl($"{Settings.RestEndpoint}/{Constants.ChatHub.Route}", options =>
                {
                    options.Headers.Add(Constants.UserToken, Settings.Token);
                    options.HttpMessageHandlerFactory = message =>
                    {
                        if (message is HttpClientHandler clientHandler)
                            // always ignore the SSL certificate
                            clientHandler.ServerCertificateCustomValidationCallback +=
                                (sender, certificate, chain, sslPolicyErrors) => true;
                        return message;
                    };
                })
                .Build();
            await _hub.StartAsync();
            _hub.Closed += async error =>
            {
                SyslogService.Warn("Chat Hub connection was closed. Trying to reconnect");
                await Task.Delay(new Random().Next(0, 5) * 1000);
                await _hub.StartAsync();
            };
            _hub.On<string, string>(Constants.ChatHub.ChatSent, (username, message) =>
            {
                if (!string.IsNullOrWhiteSpace(Chat))
                    Chat += Environment.NewLine;
                Chat = $"{Chat}{username}: {message}";
            });
            SyslogService.Information("Chat Hub created and started");
        }
    }
}