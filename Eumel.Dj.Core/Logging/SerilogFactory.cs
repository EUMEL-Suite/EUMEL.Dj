using System;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.Syslog;

namespace Eumel.Dj.Core.Logging
{
    public class SerilogFactory : ILoggerFactory
    {
        public IEumelLogger Build(ILoggerSettings settings)
        {
            if (settings.AllLoggersAreDisabled())
                return new EmptyLogger();

            var builder = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .Enrich.FromLogContext();

            // TODO RENEW WITH CDS

            //Use syslog over tcp for logging if enabled
            if (settings.Syslog.EnableSyslogLogging)
            {

                if (settings.Syslog.UseUdp)
                {
                    builder = builder.WriteTo.UdpSyslog(settings.Syslog.SysLogServerIp, restrictedToMinimumLevel: GetLevel(settings.Syslog.MinimumLevel),
                        format: SyslogFormat.Local, outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss}\t" + settings.DeviceName.PadRight(18)[..18] + "\t{Level:u5}\t{Message}");
                }
                else
                {
                    var tcpConfig = new SyslogTcpConfig
                    {
                        Host = settings.Syslog.SysLogServerIp,
                        Port = settings.Syslog.SyslogServerPort,
                        Formatter = new Rfc5424Formatter(),
                        Framer = new MessageFramer(FramingType.OCTET_COUNTING),
                        SecureProtocols = SslProtocols.Tls11 | SslProtocols.Tls12,
                        CertProvider = new CertificateFileProvider(settings.Syslog.CertificatePath,
                            settings.Syslog.CertificatePassword),
                        CertValidationCallback = (sender, certificate, chain, sslPolicyErrors) =>
                        {
                            //Verify the certificate here
                            var localCert = new X509Certificate2(certificate);
                            return localCert.Verify();
                        }
                    };
                    builder = builder.WriteTo.TcpSyslog(tcpConfig,
                        restrictedToMinimumLevel: GetLevel(settings.Syslog.MinimumLevel));
                }
            }

            if (settings.Filelog.EnableFileLogging)
                builder = builder.WriteTo.File(
                    settings.Filelog.FilePath,
                    GetLevel(settings.Filelog.MinimumLevel),
                    rollingInterval: RollingInterval.Day,
                    retainedFileCountLimit: settings.Filelog.RetainedFileCountLimit,
                    outputTemplate: settings.Filelog.OutputTemplate,
                    shared: settings.Filelog.Async,
                    fileSizeLimitBytes: settings.Filelog.FileSizeLimitBytes,
                    rollOnFileSizeLimit: settings.Filelog.RollOnFileSizeLimit
                );

            //if (new ModeDetector().IsDebug) builder = builder.WriteTo.Sink(new ConsoleSink());

            return new SerilogAdapter(builder.CreateLogger());
        }

        private LogEventLevel GetLevel(string level)
        {
            return (LogEventLevel)Enum.Parse(typeof(LogEventLevel), level ?? LogEventLevel.Information.ToString());
        }
    }
}