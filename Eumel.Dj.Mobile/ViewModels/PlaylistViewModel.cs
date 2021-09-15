using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Eumel.Dj.Mobile.Models;
using Eumel.Dj.Mobile.Services;
using Eumel.Dj.Mobile.Views;
using Microsoft.AspNetCore.SignalR.Client;
using Xamarin.Forms;

namespace Eumel.Dj.Mobile.ViewModels
{
    public class PlaylistViewModel : BaseViewModel
    {
        private HubConnection _hub;
        protected IPlaylistService PlaylistService => DependencyService.Get<IPlaylistService>();
        protected IPlayerService PlayerService => DependencyService.Get<IPlayerService>();
        protected ISyslogService LogService => DependencyService.Get<ISyslogService>();

        public ObservableCollection<PlaylistSongItem> Items { get; }

        public Command PlayPlayerCommand { get; }

        public Command<PlaylistSongItem> ItemTapped { get; }

        public Command LoadPlaylistCommand { get; }

        public PlaylistViewModel()
        {
            Title = "Playlist";
            Items = new ObservableCollection<PlaylistSongItem>();
            LoadPlaylistCommand = new Command(async () => { await ExecuteLoadPlaylistCommand(); });
            PlayPlayerCommand = new Command(async () => await PlayerService.Play(), () => Items.All(x => x.Type != SongType.Current));
            ItemTapped = new Command<PlaylistSongItem>(OnItemSelected);
        }

        private async Task ExecuteLoadPlaylistCommand()
        {
            IsBusy = true;

            try
            {
                Items.Clear();
                var playlist = await PlaylistService.Get();
                playlist.Songs.Where(x => x?.Id != null).ToList().ForEach(Items.Add);
            }
            catch (Exception ex)
            {
                LogService.Error(ex.Message);
            }
            finally
            {
                IsBusy = false;
            }
        }

        public async void OnAppearing()
        {
            IsBusy = true;
            if (string.IsNullOrWhiteSpace(DependencyService.Get<ISettingsService>().RestEndpoint))
                return;

            if (_hub == null)
            {
                _hub = new HubConnectionBuilder()
                    .WithUrl($"{DependencyService.Get<ISettingsService>().RestEndpoint}/{Constants.PlaylistHub.Route}", options =>
                    {
                        options.Headers.Add(Constants.UserToken, DependencyService.Get<ISettingsService>().RestEndpoint);
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

                _hub.On<DjPlaylist>(Constants.PlaylistHub.PlaylistChanged, pl =>
                {
                    Items.Clear();
                    var songs = pl.PastSongs.Select(x => x.ToPlaylistSongItem(SongType.Past, DependencyService.Get<ISettingsService>()))
                        .Append(pl.CurrentSong.ToPlaylistSongItem(SongType.Current, DependencyService.Get<ISettingsService>()))
                        .Concat(pl.UpcomingSongs.Select(x => x.ToPlaylistSongItem(SongType.Upcomming, DependencyService.Get<ISettingsService>())));
                    songs.Where(y => y?.Id != null).ToList().ForEach(Items.Add);
                });
            };
            if (_hub.State == HubConnectionState.Disconnected)
                await _hub.StartAsync();
        }

        private async void OnItemSelected(PlaylistSongItem songItem)
        {
            if (songItem == null)
                return;

            await Shell.Current.GoToAsync($"{nameof(SongDetailPage)}?{nameof(SongDetailViewModel.ItemId)}={songItem.Id}");
        }
    }
}