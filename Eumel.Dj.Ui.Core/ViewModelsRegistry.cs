using System.Linq;
using Caliburn.Micro;
using Eumel.Dj.Ui.Core.Interfaces;
using Eumel.Dj.Ui.Core.ViewModels;
using StructureMap;
using StructureMap.Graph;
using StructureMap.Graph.Scanning;

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