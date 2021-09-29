using System.Collections.Generic;
using Eumel.Dj.Core.Models;

namespace Eumel.Dj.Core.Messages
{
    public class SearchSongsMessage : MessageRequest<IEnumerable<Song>>
    {
        public string SearchText { get; }

        public SearchSongsMessage(object sender, string searchText) : base(sender)
        {
            SearchText = searchText;
        }
    }
}