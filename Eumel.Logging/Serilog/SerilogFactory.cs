using Eumel.Core.Logging;
using Serilog;
using Serilog.Exceptions;

namespace Eumel.Logging.Serilog
{
    public class SerilogFactory : ILoggerFactory
    {
        public IEumelLogger Build(LoggerSettings settings)
        {
            if (settings.AllLoggersAreDisabled())
                return new EmptyLogger();

            var builder = CreateBuilder(settings)
                .AddSyslogUdp(settings)
                .AddSyslogTcp(settings)
                .AddConsole(settings)
                .AddFile(settings);

            return new SerilogAdapter(builder.CreateLogger());
        }

        private static LoggerConfiguration CreateBuilder(LoggerSettings settings)
        {
            var builder = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .Enrich.WithProcessId();

            if (settings.AtLeastOneVerbose()) builder = builder.MinimumLevel.Verbose();

            if (settings.UseExtendedDebug)
                builder = builder
                    .Enrich.WithExceptionDetails()
                    .Enrich.WithMachineName()
                    .Enrich.WithEnvironmentUserName();
            if (settings.UseDebug)
                builder = builder.WriteTo.Debug();

            return builder;
        }
    }
}