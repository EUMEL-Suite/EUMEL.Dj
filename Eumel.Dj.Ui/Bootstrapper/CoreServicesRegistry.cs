using Accessibility;
using Caliburn.Micro;
using StructureMap;
using TinyMessenger;

namespace Eumel.Dj.Ui.Bootstrapper
{
    public class CoreServicesRegistry : Registry
    {
        public CoreServicesRegistry()
        {
            _ = For<IWindowManager>().Use(new WindowManager());
            _ = For<ITinyMessengerHub>().Use(TinyMessengerHub.DefaultHub);
        }
    }
}