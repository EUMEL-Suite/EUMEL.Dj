namespace Eumel.Dj.Mobile.Models
{
    public class PlaylistSongItem
    {
        public string Id { get; set; }
        public string Text { get; set; }
        public string Artist { get; set; }
        public int VoteCount { get; set; }
        public bool VotedByMe { get; set; }
    }
}