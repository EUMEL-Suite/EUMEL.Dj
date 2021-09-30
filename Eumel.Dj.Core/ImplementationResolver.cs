using System;
using System.Collections.Generic;
using System.Linq;
using Eumel.Dj.Core.Exceptions;
using Eumel.Dj.Core.Models;

namespace Eumel.Dj.Core
{
    public class ImplementationResolver<T> : IImplementationResolver<T>
    {
        private readonly ImplementationSettings _setting;
        private readonly IEnumerable<T> _registeredClasses;

        // it is better to add the structuremap context so an instance is not created before and request by name
        public ImplementationResolver(ImplementationSettings setting, IEnumerable<T> registeredClasses)
        {
            _setting = setting ?? throw new ArgumentNullException(nameof(setting));
            _registeredClasses = registeredClasses ?? throw new ArgumentNullException(nameof(registeredClasses));
        }

        public T Resolve()
        {
            var interfaceName = typeof(T).Name;

            var settingProperty = typeof(ImplementationSettings).GetProperty(interfaceName) ?? throw new EumelDjException($"A configuration for interface {interfaceName} could not be found on settings class {nameof(ImplementationSettings)}");
            var setting = settingProperty.GetValue(_setting) as string;

            var instance = _registeredClasses.SingleOrDefault(x => x.GetType().Name == setting) ?? throw new EumelDjException($"Configured class {setting} for interface {interfaceName} was not found in registered classes");

            return instance;
        }
    }
}