using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Windows;
using System.Windows.Media;
using ITunesLibraryParser;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using TinyMessenger;
using WebApplication1;
using WebApplication1.Controllers;

namespace Eumel.Dj.Ui
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private IWebHost _host;
        private readonly TinyMessengerHub _hub;
        private MediaPlayer _mediaPlayer = new MediaPlayer();
        private readonly ITunesLibrary _itunes;

        public MainWindow()
        {
            InitializeComponent();

            _hub = TinyMessengerHub.DefaultHub;
            _hub.Subscribe((Action<SomeMessage>)HandlesomeMessage);
            _hub.Subscribe((Action<PlaylistMessage>)GetPlaylist);
            _hub.Subscribe((Action<PlaysongMessage>)PlaySong);

            _itunes = new ITunesLibrary(@"C:\Users\thoma\Music\iTunes\iTunes Music Library.xml");
        }

        private void PlaySong(PlaysongMessage msg)
        {
            Application.Current.Dispatcher.Invoke(new Action(() =>
           {
               _mediaPlayer.Open(new Uri(msg.Location));
               _mediaPlayer.Play();
           }));
        }

        private void GetPlaylist(PlaylistMessage mgs)
        {
            mgs.Playlist = _itunes.Tracks.Select(x => new EumelTrack() { Name = x.Name, Album = x.Album, Artist = x.Artist, AlbumArtist = x.Album, Location = Uri.UnescapeDataString(x.Location.Replace("file://localhost/", "", StringComparison.InvariantCulture)) }).ToArray();
        }

        private void HandlesomeMessage(SomeMessage msg)
        {
            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                Log.Text = msg.Message + Environment.NewLine + Log.Text;
            }));
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
