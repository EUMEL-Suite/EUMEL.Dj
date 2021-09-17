using Eumel.Dj.Ui.Core.Interfaces;
using Eumel.Dj.Ui.Core.ViewModels;
using StructureMap;

namespace Eumel.Dj.Ui.Bootstrapper
{
    public class ViewModelsRegistry : Registry
    {
        public ViewModelsRegistry()
        {
            _ = For<IShellViewModel>().Use<ShellViewModel>().AlwaysUnique();
        }
    }
}