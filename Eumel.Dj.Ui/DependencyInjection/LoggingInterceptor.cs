using System;
using System.Diagnostics;
using System.Linq;
using Castle.DynamicProxy;
using Eumel.Core.Logging;

namespace Eumel.Dj.Ui.DependencyInjection
{
    public class LoggingInterceptor : IInterceptor
    {
        private readonly IEumelLogger _logger;

        public LoggingInterceptor(IEumelLogger logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public void Intercept(IInvocation invocation)
        {
            var watch = Stopwatch.StartNew();
            var enteringLogMessage = $"[Entering Method] [#{invocation.InvocationTarget.GetHashCode()}] {invocation.TargetType.Name}.{invocation.Method.Name}";

            if (invocation.Arguments.Length > 0)
                enteringLogMessage += $" ({string.Join(", ", invocation.Arguments)})";
            _logger.Verbose(enteringLogMessage);

            try
            {
                //Run the actual Invocation
                invocation.Proceed();
            }
            catch (Exception ex)
            {
                var exceptionLogMessage = $"[Catching Method] [#{invocation.InvocationTarget.GetHashCode()}] {invocation.TargetType.Name}.{invocation.Method.Name}: Caught Exception of type {ex.GetType()}";
                _logger.Error(exceptionLogMessage, ex);
                throw;
            }
            finally
            {
                watch.Stop();
                _logger.Verbose($"[Leaving  Method] [#{invocation.InvocationTarget.GetHashCode()}] {invocation.TargetType.Name}.{invocation.Method.Name} after {watch.ElapsedMilliseconds} Milliseconds");
            }
        }
    }
}