using Caliburn.Micro;
using Eumel.Dj.Ui.Core.Interfaces;
using StructureMap;

namespace Eumel.Dj.Ui.Core
{
    public class ViewModelsRegistry : Registry
    {
        public ViewModelsRegistry()
        {
            Scan(_ =>
            {
                _.TheCallingAssembly();
                _.WithDefaultConventions(); // Default I[Name]/[Name] registration convention

                // add all screens and shells
                _.AddAllTypesOf<IScreen>();
                _.AddAllTypesOf<IShellViewModel>();
            });
        }
    }
}