using System;
using Caliburn.Micro;
using TinyMessenger;

namespace Eumel.Dj.Ui.Core.ViewModels
{
    public class ShellViewModel : PropertyChangedBase
    {
        private readonly ITinyMessengerHub _hub;

        //public ShellViewModel(ITinyMessengerHub hub)
        //{
        //    _hub = hub ?? throw new ArgumentNullException(nameof(hub));
        //}

        string name;

        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                NotifyOfPropertyChange(() => Name);
                NotifyOfPropertyChange(() => CanSayHello);
            }
        }

        public bool CanSayHello
        {
            get { return !string.IsNullOrWhiteSpace(Name); }
        }

        public void SayHello()
        {
            Console.WriteLine($"Hello {name}");
        }
    }
}
