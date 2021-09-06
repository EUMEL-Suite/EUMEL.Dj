using System;
using System.Collections.Generic;
using System.Linq;
using Eumel.Dj.WebServer;
using Eumel.Dj.WebServer.Messages;
using Eumel.Dj.WebServer.Models;
using ITunesLibraryParser;
using Microsoft.Extensions.Logging;
using TinyMessenger;

namespace Eumel.Dj.Ui.Services
{
    public class ItunesProviderService : IPlaylistProviderService, IDisposable
    {
        private readonly Settings _settings;
        private readonly ITinyMessengerHub _hub;
        private readonly ITunesLibrary _itunes;
        private readonly List<TinyMessageSubscriptionToken> _tinyMessageSubscriptions;

        public ItunesProviderService(Settings settings, ITinyMessengerHub hub)
        {
            _settings = settings;
            _hub = hub;
            _itunes = new ITunesLibrary(settings.ItunesLibrary);

            _tinyMessageSubscriptions = new List<TinyMessageSubscriptionToken>(new[]
            {
                hub.Subscribe((Action<GetSongsMessage>)GetSongs),
                hub.Subscribe((Action<GetSongsSourceMessage>)GetSongsSource)
            });
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

        // cache songs!
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
                    Id = x.PersistentId,
                    Album = x.Album,
                    Artist = x.Artist,
                    AlbumArtist = x.Album
                }).ToArray();
            return songs;
        }

        public Uri GetLocationOfSongById(string songId)
        {
            var playlist = string.IsNullOrWhiteSpace(_settings.SelectedPlaylist)
                ? _itunes.Playlists.First() // this can be all songs?
                : _itunes.Playlists.Single(x =>
                    string.Compare(x.Name, _settings.SelectedPlaylist, StringComparison.InvariantCultureIgnoreCase) == 0);

            // iTunes has an interesting format
            var location = playlist.Tracks
                 .Where(x => string.Compare(x.PersistentId, songId, StringComparison.OrdinalIgnoreCase) == 0)
                 .Select(x => Uri.UnescapeDataString(x.Location.Replace("file://localhost/", "", StringComparison.InvariantCulture)))
                 .FirstOrDefault()
                           ?? throw new SongNotFoundDjException($"Song {songId} was not found in playlist {playlist}");
            return new Uri(location);
        }

        public Song FindSongById(string songId)
        {
            if (songId == null)
                return null;

            var playlist = string.IsNullOrWhiteSpace(_settings.SelectedPlaylist)
                ? _itunes.Playlists.First() // this can be all songs?
                : _itunes.Playlists.Single(x =>
                    string.Compare(x.Name, _settings.SelectedPlaylist, StringComparison.InvariantCultureIgnoreCase) == 0);

            var result = playlist.Tracks
                .Where(x => string.Compare(x.PersistentId, songId, StringComparison.OrdinalIgnoreCase) == 0)
                .Select(x => new Song()
                {
                    Name = x.Name,
                    Id = x.PersistentId,
                    Album = x.Album,
                    Artist = x.Artist,
                    AlbumArtist = x.Album
                }).FirstOrDefault()
                         ?? throw new SongNotFoundDjException($"Song {songId} was not found in playlist {playlist}"); ;

            return result;
        }

        public void Dispose()
        {
            _tinyMessageSubscriptions.ForEach(x => _hub.Unsubscribe(x));
        }
    }
}