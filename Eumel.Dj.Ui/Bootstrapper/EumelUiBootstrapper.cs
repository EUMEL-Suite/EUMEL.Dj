using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows;
using Caliburn.Micro;
using Eumel.Dj.Ui.AutoStartServices;
using Eumel.Dj.Ui.Core.ViewModels;
using StructureMap;

namespace Eumel.Dj.Ui.Bootstrapper
{
    public class EumelUiBootstrapper : BootstrapperBase
    {
        private Container _container;
        private IEnumerable<IAutoStart> _autoStartupServices;

        public EumelUiBootstrapper()
        {
            Initialize();
        }

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            DisplayRootViewFor<ShellViewModel>();

            // here we start all services which implement the autostart interface
            _autoStartupServices = GetAllInstances(typeof(IAutoStart)).Cast<IAutoStart>();
            _autoStartupServices.ToList().ForEach(x => x.Start());
        }

        protected override IEnumerable<Assembly> SelectAssemblies()
        {
            return base.SelectAssemblies()
                .Append(Assembly.GetAssembly(typeof(ShellViewModel)));
        }

        protected override void Configure()
        {
            _container = new Container(_ =>
            {
                _.AddRegistry<CoreServicesRegistry>();
                _.AddRegistry<ViewModelsRegistry>();
            });
        }

        protected override object GetInstance(Type service, string key)
        {
            return key == null
                ? _container.GetInstance(service)
                : _container.GetInstance(service, key);
        }

        protected override IEnumerable<object> GetAllInstances(Type service)
        {
            return _container.GetAllInstances(service).Cast<object>();
        }

        protected override void OnExit(object sender, EventArgs e)
        {
            _autoStartupServices.ToList().ForEach(x => x.Stop());
            _autoStartupServices.OfType<IDisposable>().ToList().ForEach(x => x.Dispose());

            base.OnExit(sender, e);
        }
    }
}
