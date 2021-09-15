using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Eumel.Dj.Mobile.Models;
using Microsoft.AspNetCore.SignalR.Client;
using Xamarin.Forms;

namespace Eumel.Dj.Mobile.ViewModels
{
    public class PlaylistViewModel : BaseViewModel
    {
        private HubConnection _hub;

        public ObservableCollection<PlaylistSongItem> Items { get; }

        public Command<PlaylistSongItem> ItemTapped { get; }

        public Command LoadPlaylistCommand { get; }

        public PlaylistViewModel()
        {
            Title = "Playlist";
            Items = new ObservableCollection<PlaylistSongItem>();
            LoadPlaylistCommand = new Command(async () => { await ExecuteLoadPlaylistCommand(); });
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
                SyslogService.Error(ex.Message);
            }
            finally
            {
                IsBusy = false;
            }
        }

        public async void OnAppearing()
        {
            IsBusy = true;
            if (string.IsNullOrWhiteSpace(Settings.RestEndpoint))
                return;

            if (_hub == null)
            {
                _hub = new HubConnectionBuilder()
                    .WithUrl($"{Settings.RestEndpoint}/{Constants.PlaylistHub.Route}", options =>
                    {
                        options.Headers.Add(Constants.UserToken, Settings.RestEndpoint);
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
                    var songs = pl.PastSongs.Select(x => x.ToPlaylistSongItem(SongType.Past, Settings))
                        .Append(pl.CurrentSong.ToPlaylistSongItem(SongType.Current, Settings))
                        .Concat(pl.UpcomingSongs.Select(x => x.ToPlaylistSongItem(SongType.Upcomming, Settings)));
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

            // we don't navigation to any detail page any longer
            //await Shell.Current.GoToAsync($"{nameof(SongDetailPage)}?{nameof(SongDetailViewModel.ItemId)}={songItem.Id}");
        }
    }
}