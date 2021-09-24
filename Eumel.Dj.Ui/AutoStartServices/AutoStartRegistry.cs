using StructureMap;

namespace Eumel.Dj.Ui.AutoStartServices
{
    public class AutoStartRegistry : Registry
    {
        public AutoStartRegistry()
        {
            Scan(_ =>
            {
                _.TheCallingAssembly();
                _.RegisterConcreteTypesAgainstTheFirstInterface(); // Register all concrete types against the first interface (if any) that they implement

                // add all screens and shells
                _.AddAllTypesOf<IAutoStart>();
            });
        }
    }
}