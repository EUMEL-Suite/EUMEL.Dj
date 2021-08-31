using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Windows;
using System.Windows.Media;
using Eumel.Dj.WebServer;
using Eumel.Dj.WebServer.Messages;
using Eumel.Dj.WebServer.Models;
using ITunesLibraryParser;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using TinyMessenger;

namespace Eumel.Dj.Ui
{
    public class DjService
    {
        private readonly ITinyMessengerHub _hub;
        private readonly IPlaylistProviderService _playlistService;
        private MediaPlayer _mediaPlayer = new();

        public DjService(ITinyMessengerHub hub, IPlaylistProviderService playlistService)
        {
            _hub = hub;
            _playlistService = playlistService;

            hub.Subscribe((Action<PlayerMessage>)PlayerRequest);
            hub.Subscribe((Action<VoteMessage>)Vote);
            hub.Subscribe((Action<GetPlaylistMessage>)GetPlaylist);
            hub.Subscribe((Action<GetMyVotesMessage>)GetMyVotes);
        }

        private void GetMyVotes(GetMyVotesMessage message)
        {

        }

        private void GetPlaylist(GetPlaylistMessage message)
        {

        }

        private void Vote(VoteMessage message)
        {

        }

        private void PlayerRequest(PlayerMessage message)
        {
            _mediaPlayer.Dispatcher.Invoke(() =>
            {
                switch (message.PlayerAction)
                {
                    case PlayerMessage.PlayerControl.Play:
                        if (!string.IsNullOrWhiteSpace(message.Location))
                            _mediaPlayer.Open(new Uri(message.Location));
                        _mediaPlayer.Play();
                        break;
                    case PlayerMessage.PlayerControl.Pause:
                        _mediaPlayer.Pause();
                        break;
                    case PlayerMessage.PlayerControl.Continue:
                        _mediaPlayer.Play();
                        break;
                    case PlayerMessage.PlayerControl.Stop:
                        _mediaPlayer.Stop();
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                message.Response = new MessageResponse<bool>(true);
            });
        }
    }

    public class ItunesProviderService : IPlaylistProviderService
    {
        private readonly Settings _settings;
        private readonly ITunesLibrary _itunes;

        public ItunesProviderService(Settings settings, ITinyMessengerHub hub)
        {
            _settings = settings;
            _itunes = new ITunesLibrary(settings.ItunesLibrary);

            hub.Subscribe((Action<GetSongsMessage>)GetSongs);
            hub.Subscribe((Action<GetSongsSourceMessage>)GetSongsSource);
        }

        private void GetSongsSource(GetSongsSourceMessage message)
        {
            var playlist = string.IsNullOrWhiteSpace(_settings.SelectedPlaylist)
                ? _itunes.Playlists.First() // this can be all songs?
                : _itunes.Playlists.Single(x => string.Compare(x.Name, _settings.SelectedPlaylist, StringComparison.InvariantCultureIgnoreCase) == 0);

            message.Response = new MessageResponse<SongsSource>(new SongsSource(playlist.Name, playlist.Tracks.Count()));
        }

        private void GetSongs(GetSongsMessage message)
        {
            var playlist = string.IsNullOrWhiteSpace(_settings.SelectedPlaylist)
                ? _itunes.Playlists.First() // this can be all songs?
                : _itunes.Playlists.Single(x => string.Compare(x.Name, _settings.SelectedPlaylist, StringComparison.InvariantCultureIgnoreCase) == 0);

            var songs = playlist.Tracks
                .Skip(message.Skip)
                .Take(message.Take)
                .Select(x => new Song()
                {
                    Name = x.Name,
                    Album = x.Album,
                    Artist = x.Artist,
                    AlbumArtist = x.Album,
                    Location = Uri.UnescapeDataString(x.Location.Replace("file://localhost/", "", StringComparison.InvariantCulture)) // iTunes has an interesting format
                }).ToArray();

            message.Response = new MessageResponse<IEnumerable<Song>>(songs);
        }
    }

    public interface IPlaylistProviderService
    {
    }

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private IWebHost _host;
        private readonly TinyMessengerHub _hub;
        private readonly ITunesLibrary _itunes;
        private readonly DjService _djService;
        private readonly IPlaylistProviderService _playlistProviderService;

        public MainWindow()
        {
            InitializeComponent();

            _hub = TinyMessengerHub.DefaultHub;

            _playlistProviderService = new ItunesProviderService(Settings.Default, _hub);
            _djService = new DjService(_hub, _playlistProviderService);

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
                    .ConfigureServices((context, services) => { services.AddSingleton<ITinyMessengerHub>(_hub); })
                    .Build();
                _host.RunAsync();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                throw;
            }
        }

        public static IEnumerable<string[]> ReadPlaylist(string docPath)
        {
            var contents = File.ReadAllLines(docPath).Skip(1);

            foreach (var content in contents)
                yield return content.Split("\t");
        }
    }
}
