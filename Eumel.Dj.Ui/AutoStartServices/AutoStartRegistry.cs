using Castle.DynamicProxy;
using Eumel.Dj.Ui.DependencyInjection;
using Eumel.Logging;
using StructureMap;
using TinyMessenger;

namespace Eumel.Dj.Ui.AutoStartServices
{
    public class AutoStartRegistry : Registry
    {
        public AutoStartRegistry()
        {
            Scan(_ =>
            {
                _.TheCallingAssembly();

                // Register all concrete types against the first interface (if any) that they implement
                _.RegisterConcreteTypesAgainstTheFirstInterface();

                // add all auto start implementations
                _.AddAllTypesOf<IAutoStart>();
            });
        }
    }
}