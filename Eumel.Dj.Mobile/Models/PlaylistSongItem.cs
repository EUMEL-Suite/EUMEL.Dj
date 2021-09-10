namespace Eumel.Dj.Mobile.Models
{
    public enum SongType
    {
        Past,
        Current,
        Upcomming
    }

    public class PlaylistSongItem
    {
        public string Id { get; set; }
        public int VoteCount { get; set; }
        public bool VotedByMe { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public SongType Type { get; set; }

        public bool HasVotes => VoteCount > 0;

    }
}