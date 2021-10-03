using System.Windows;
using TinyMessenger;

namespace Eumel.Dj.Ui
{
    public partial class App : Application
    {
        private TinyMessengerHub _hub;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            _hub = TinyMessengerHub.DefaultHub;
        }
    }
}