
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows;
using Caliburn.Micro;
using Eumel.Dj.Ui.Core.ViewModels;

namespace Eumel.Dj.Ui.Core.Bootstrapper
{
    public class EumelUiBootstrapper : BootstrapperBase
    {
        public EumelUiBootstrapper()
        {
            Initialize();
        }

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            DisplayRootViewFor<ShellViewModel>();
        }

        protected override IEnumerable<Assembly> SelectAssemblies()
        {
            return base.SelectAssemblies()
                .Append(Assembly.GetAssembly(typeof(ShellViewModel)));
        }
    }
}
