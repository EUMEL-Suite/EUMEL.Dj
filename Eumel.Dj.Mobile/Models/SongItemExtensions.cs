namespace Eumel.Dj.Mobile.Models
{
    public static class SongItemExtensions
    {
        public static SongItem ToSongItem(this Song source, bool hasMyVote)
        {
            return new SongItem()
            {
                Id = source.Id,
                Title = source.Name,
                Description = $"{source.Artist} - {source.Album}",
                //Voters = string.Join(","source.Voters),
                HasMyVote = hasMyVote
            };
        }
    }
}