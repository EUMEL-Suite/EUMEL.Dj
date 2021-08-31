using System;
using System.Collections.Generic;
using System.Linq;
using Eumel.Dj.WebServer.Messages;
using Eumel.Dj.WebServer.Models;
using ITunesLibraryParser;
using TinyMessenger;

namespace Eumel.Dj.Ui
{
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
            var songs = GetSongs(message.Skip, message.Take);

            message.Response = new MessageResponse<IEnumerable<Song>>(songs.ToArray());
        }

        public IEnumerable<Song> GetSongs(int skip = 0, int take = int.MaxValue)
        {
            var playlist = string.IsNullOrWhiteSpace(_settings.SelectedPlaylist)
                ? _itunes.Playlists.First() // this can be all songs?
                : _itunes.Playlists.Single(x =>
                    string.Compare(x.Name, _settings.SelectedPlaylist, StringComparison.InvariantCultureIgnoreCase) == 0);

            var songs = playlist.Tracks
                .Skip(skip)
                .Take(take)
                .Select(x => new Song()
                {
                    Name = x.Name,
                    Album = x.Album,
                    Artist = x.Artist,
                    AlbumArtist = x.Album,
                    Location = Uri.UnescapeDataString(x.Location.Replace("file://localhost/", "",
                        StringComparison.InvariantCulture)) // iTunes has an interesting format
                }).ToArray();
            return songs;
        }
    }
}