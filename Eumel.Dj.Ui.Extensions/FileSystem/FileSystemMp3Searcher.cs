using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using Eumel.Dj.Core;
using Eumel.Dj.Core.Messages;
using Eumel.Dj.Core.Models;
using Microsoft.Extensions.Logging;
using TinyMessenger;

namespace Eumel.Dj.Ui.Extensions.FileSystem
{
    internal class FileSystemMp3Searcher : ISongsProviderService
    {
        private record SongWrapper(Song Song, string Id, Uri Location);

        private readonly IAppSettings _settings;
        private readonly ITinyMessengerHub _hub;
        private IList<SongWrapper> _cache;


        public FileSystemMp3Searcher(IAppSettings settings, ITinyMessengerHub hub)
        {
            _settings = settings ?? throw new ArgumentNullException(nameof(settings));
            _hub = hub;
        }

        public IEnumerable<Song> GetSongs(int skip = 0, int take = int.MaxValue)
        {
            if (_cache == null) InitSongCache();

            return _cache.Skip(skip).Take(take).Select(x => x.Song).ToArray();
        }

        public Uri GetLocationOfSongById(string songId)
        {
            if (_cache == null) InitSongCache();

            return _cache.Single(x => x.Id == songId).Location;
        }

        public Song FindSongById(string songId)
        {
            if (_cache == null) InitSongCache();

            return _cache.Single(x => x.Id == songId).Song;
        }

        public SongsSource GetSourceInfo()
        {
            if (_cache == null) InitSongCache();

            return new SongsSource(new DirectoryInfo(_settings.SongsPath).Name, _cache.Count);
        }

        public IEnumerable<Song> SearchSongs(string query, int limit, out int totalNumberOfSongs)
        {
            var found = GetSongs().Where(song => song.Album.StartsWith(query, StringComparison.CurrentCultureIgnoreCase) ||
                                                 song.Artist.StartsWith(query, StringComparison.CurrentCultureIgnoreCase) ||
                                                 song.Name.StartsWith(query, StringComparison.CurrentCultureIgnoreCase)).ToArray();
            totalNumberOfSongs = found.Length;
            return found.Take(limit).ToArray();
        }


        [MethodImpl(MethodImplOptions.Synchronized)]
        private void InitSongCache()
        {
            var source = new DirectoryInfo(_settings.SongsPath);
            var items = source.GetFiles("*.mp3", SearchOption.AllDirectories);
            _cache = new List<SongWrapper>();

            foreach (var item in items)
            {
                var mp3 = TagLib.File.Create(item.FullName);
                var id = Guid.NewGuid().ToString();
                _cache.Add(new SongWrapper(new Song
                {
                    Name = mp3.Tag.Title ?? string.Empty,
                    Album = mp3.Tag.Album ?? string.Empty,
                    Artist = mp3.Tag.JoinedPerformers?? mp3.Tag.JoinedAlbumArtists ?? string.Empty,
                    AlbumArtist = mp3.Tag.JoinedAlbumArtists ?? mp3.Tag.JoinedPerformers ?? string.Empty,
                    Id = id
                }, id, new Uri(item.FullName)));
            }

            _hub.Publish(new LogMessage(this, $"{nameof(FileSystemMp3Searcher)} initialized song cache with {_cache.Count} songs on directory {source.FullName}", LogLevel.Information));
            _hub.Publish(new LogMessage(this, $"The song source contains more than {Constants.RequiredSearchLimit} songs. The frontend will require to search for finding songs", LogLevel.Warning));
        }
    }
}
