using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows;
using Caliburn.Micro;
using Eumel.Core.Extensions;
using Eumel.Dj.Ui.AutoStartServices;
using Eumel.Dj.Ui.Core;
using Eumel.Dj.Ui.Core.Interfaces;
using Eumel.Dj.Ui.Core.ViewModels;
using Eumel.Dj.Ui.Extensions;
using StructureMap;

namespace Eumel.Dj.Ui.DependencyInjection
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
            DisplayRootViewFor<IShellViewModel>();

            // here we start all services which implement the auto-start interface
            _autoStartupServices = GetAllInstances(typeof(IAutoStart)).Cast<IAutoStart>();
            _autoStartupServices.ForEach(x => x.Start());
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
                _.AddRegistry<AutoStartRegistry>();
                _.AddRegistry<ViewModelsRegistry>();
                _.AddRegistry<ExtensionsServicesRegistry>();
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
            _autoStartupServices.ForEach(x => x.Stop());
            _autoStartupServices.OfType<IDisposable>().ForEach(x => x.Dispose());

            base.OnExit(sender, e);
        }
    }
}
