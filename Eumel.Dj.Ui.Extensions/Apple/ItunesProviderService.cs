using System;
using System.Collections.Generic;
using System.Linq;
using Eumel.Dj.Core;
using Eumel.Dj.Core.Exceptions;
using Eumel.Dj.Core.Models;
using ITunesLibraryParser;

namespace Eumel.Dj.Ui.Extensions.Apple
{
    internal class ItunesProviderService : ISongsProviderService
    {
        private readonly ITunesLibrary _itunes;
        private readonly IAppSettings _settings;

        public ItunesProviderService(IAppSettings settings)
        {
            _settings = settings;
            _itunes = new ITunesLibrary(settings.ItunesLibrary);
        }

        // cache songs!
        public IEnumerable<Song> GetSongs(int skip = 0, int take = int.MaxValue)
        {
            var playlist = SelectedPlaylist;

            var songs = playlist.Tracks
                .Skip(skip)
                .Take(take)
                .Select(ToSong).ToArray();
            return songs;
        }

        private static Song ToSong(Track x)
        {
            return new Song
            {
                Name = x.Name,
                Id = x.PersistentId,
                Album = x.Album,
                Artist = x.Artist,
                AlbumArtist = x.Album
            };
        }

        public Uri GetLocationOfSongById(string songId)
        {
            var playlist = SelectedPlaylist;

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

            var playlist = SelectedPlaylist;

            var result = playlist.Tracks
                             .Where(x => string.Compare(x.PersistentId, songId, StringComparison.OrdinalIgnoreCase) == 0)
                             .Select(ToSong).FirstOrDefault()
                         ?? throw new SongNotFoundDjException($"Song {songId} was not found in playlist {playlist}");
            ;

            return result;
        }

        public SongsSource GetSourceInfo()
        {
            var playlist = SelectedPlaylist;

            return new SongsSource(playlist.Name, playlist.Tracks.Count());
        }

        private Playlist SelectedPlaylist => _itunes.Playlists.SingleOrDefault(x => string.Compare(x.Name, _settings.SelectedPlaylist, StringComparison.InvariantCultureIgnoreCase) == 0) ?? _itunes.Playlists.First();
    }
}