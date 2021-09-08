using Xamarin.Forms;

namespace Eumel.Dj.Mobile.Models
{
    public class SongItem
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool HasMyVote { get; set; }
    }
}