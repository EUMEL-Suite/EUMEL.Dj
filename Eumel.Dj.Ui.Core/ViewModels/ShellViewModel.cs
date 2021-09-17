using System;
using Caliburn.Micro;
using Eumel.Dj.Ui.Core.Interfaces;
using TinyMessenger;

namespace Eumel.Dj.Ui.Core.ViewModels
{
    public class ShellViewModel : PropertyChangedBase, IShellViewModel
    {
        public ILogOutputViewModel LogWindow { get; }
        private readonly ITinyMessengerHub _hub;

        public ShellViewModel(ITinyMessengerHub hub, ILogOutputViewModel logWindow)
        {
            LogWindow = logWindow;
            _hub = hub ?? throw new ArgumentNullException(nameof(hub));
        }

    }
}
