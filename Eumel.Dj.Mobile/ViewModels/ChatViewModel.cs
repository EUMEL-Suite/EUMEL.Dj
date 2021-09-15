using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using Xamarin.Forms;

namespace Eumel.Dj.Mobile.ViewModels
{
    public class ChatViewModel : BaseViewModel
    {
        private string _message;
        private HubConnection _hub;
        private string _chat;
        private string _messagePlaceholder;
        public Command SendMessageCommand { get; }

        public ChatViewModel()
        {
            SendMessageCommand = new Command(SendMessageAsync);
        }

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

        public void OnAppearing()
        {
            MessagePlaceholder = $"send as {Settings.Username}...";

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
            _hub.StartAsync();
            _hub.Closed += async (error) =>
            {
                await Task.Delay(new Random().Next(0, 5) * 1000);
                await _hub.StartAsync();
            };
            _hub.On<string, string>(Constants.ChatHub.ChatSent, (username, message) =>
            {
                Chat = $"{Chat}{Environment.NewLine}{username}: {message}";
            });
        }
    }
}