using System.ComponentModel;
using System.Runtime.CompilerServices;
using Eumel.Dj.Mobile.Annotations;

namespace Eumel.Dj.Mobile.Models
{
    public class SongItem : INotifyPropertyChanged
    {
        private bool _hasMyVote;

        public string Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public bool HasMyVote
        {
            get => _hasMyVote;
            set { _hasMyVote = value; OnPropertyChanged(nameof(HasMyVote));}
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}