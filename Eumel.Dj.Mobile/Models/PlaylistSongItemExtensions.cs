using System;
using System.Linq;
using Eumel.Dj.Mobile.Services;

namespace Eumel.Dj.Mobile.Models
{
    public static class PlaylistSongItemExtensions
    {
        public static PlaylistSongItem ToPlaylistSongItem(this VotedSong source, SongType songType, ISettingsService settings)
        {
            if (source == null) return null;

            return new PlaylistSongItem()
            {
                Id = source.Id,
                Title = source.Name,
                Description = $"{source.Artist} - {source.Album}",
                VoteCount = source.Voters.Count,
                Type = songType,
                VotedByMe = source.Voters.Any(x =>
                    string.Compare(x, settings.Username, StringComparison.CurrentCultureIgnoreCase) == 0)
            };
        }
    }
}