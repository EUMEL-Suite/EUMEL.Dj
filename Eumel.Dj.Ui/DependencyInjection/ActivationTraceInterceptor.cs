using System;
using System.Collections.Generic;
using System.Linq;
using Eumel.Core.Logging;
using StructureMap.Building.Interception;
using StructureMap.Pipeline;

namespace Eumel.Dj.Ui.DependencyInjection
{
    public class ActivationTraceInterceptor : IInterceptorPolicy
    {
        private readonly IEumelLogger _logger;

        public ActivationTraceInterceptor(IEumelLogger logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public string Description { get; } = "Log and trace the activation stack";
        public IEnumerable<IInterceptor> DetermineInterceptors(Type pluginType, Instance instance)
        {
            _logger.Verbose($"Type {pluginType.Name} resolved with concrete type {instance.ReturnedType.Name}");
            return Enumerable.Empty<IInterceptor>();
        }
    }
}