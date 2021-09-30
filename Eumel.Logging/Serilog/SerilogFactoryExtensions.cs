using System;
using System.Reflection;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using Eumel.Core.Logging;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.Syslog;

namespace Eumel.Logging.Serilog
{
    public static class SerilogFactoryExtensions
    {
        public static LoggerConfiguration AddFile(this LoggerConfiguration builder, LoggerSettings settings)
        {
            if (settings.Filelog?.EnableFileLogging ?? false)
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
            return builder;
        }

        public static LoggerConfiguration WithLogLevel(this LoggerConfiguration builder, LoggerSettings settings)
        {
            var level = Enum.Parse<LogEventLevel>(settings?.ServerLogLevel ?? LogEventLevel.Information.ToString());
            return builder.MinimumLevel.Is(level);
        }

        public static LoggerConfiguration AddConsole(this LoggerConfiguration builder, LoggerSettings settings)
        {
            if (!settings.UseConsole)
                return builder;

            builder.WriteTo.Console();

            return builder;
        }

        public static LoggerConfiguration AddSyslogTcp(this LoggerConfiguration builder, LoggerSettings settings)
        {
            //Use syslog over tcp for logging if enabled
            if (settings.Syslog.EnableSyslogLogging && !settings.Syslog.UseUdp)
            {
                var tcpConfig = new SyslogTcpConfig
                {
                    Host = settings.Syslog.SysLogServerIp,
                    Port = settings.Syslog.SyslogServerPort == 0 ? 601 : settings.Syslog.SyslogServerPort,
                    Formatter = new Rfc5424Formatter(),
                    Framer = new MessageFramer(FramingType.OCTET_COUNTING)
                };

                if (!string.IsNullOrWhiteSpace(settings.Syslog.CertificatePath))
                {
                    tcpConfig.Port = settings.Syslog.SyslogServerPort == 0 ? 6514 : settings.Syslog.SyslogServerPort;
                    tcpConfig.SecureProtocols = SslProtocols.Tls11 | SslProtocols.Tls12;
                    tcpConfig.CertProvider = new CertificateFileProvider(settings.Syslog.CertificatePath, settings.Syslog.CertificatePassword);
                    tcpConfig.CertValidationCallback = (sender, certificate, chain, sslPolicyErrors) =>
                    {
                        //Verify the certificate here
                        var localCert = new X509Certificate2(certificate);
                        return localCert.Verify();
                    };
                }

                builder = builder.WriteTo.TcpSyslog(tcpConfig);
            }

            return builder;
        }

        public static LoggerConfiguration AddSyslogUdp(this LoggerConfiguration builder, LoggerSettings settings)
        {
            if (settings.Syslog.EnableSyslogLogging && settings.Syslog.UseUdp)
            {
                var appName = Assembly.GetExecutingAssembly().GetName().Name;
                builder = builder.WriteTo.UdpSyslog(
                    settings.Syslog.SysLogServerIp,
                    settings.Syslog.SyslogServerPort == 0 ? 514 : settings.Syslog.SyslogServerPort,
                    appName);
            }

            return builder;
        }

        private static LogEventLevel GetLevel(string level)
        {
            return (LogEventLevel)Enum.Parse(typeof(LogEventLevel), level);
        }
    }
}