using System;
using System.Net.Http;
using System.Threading.Tasks;
using Eumel.Dj.Mobile.Services;
using Microsoft.AspNetCore.SignalR.Client;
using Xamarin.Forms;

namespace Eumel.Dj.Mobile.ViewModels
{
    public class ChatViewModel : BaseViewModel
    {
        private string _message;
        private HubConnection _hub;
        private string _chat;
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

        private async void SendMessageAsync()
        {
            var settings = DependencyService.Get<ISettingsService>();
            try
            {
                await _hub.InvokeAsync("SendChatMessage", settings.Username, Message);
            }
            catch (Exception ex)
            {
                DependencyService.Get<ISyslogService>().Error(ex.Message);
            }
        }

        public void OnAppearing()
        {
            if (_hub != null) return;

            _hub = new HubConnectionBuilder()
                .WithUrl(DependencyService.Get<ISettingsService>().RestEndpoint + "/chatHub", options =>
                {
                    options.Headers.Add("usertoken", DependencyService.Get<ISettingsService>().Token);
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
            _hub.StartAsync();
            _hub.Closed += async (error) =>
            {
                await Task.Delay(new Random().Next(0, 5) * 1000);
                await _hub.StartAsync();
            };
            _hub.On<string, string>("ChatSent", (username, message) =>
            {
                Chat = $"{username}: {message}{Environment.NewLine}{Chat}";
            });
        }
    }
}