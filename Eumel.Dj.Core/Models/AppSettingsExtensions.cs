using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using Eumel.Core.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Eumel.Dj.Core.Models
{
    public static class AppSettingsExtensions
    {
        private static string GetLocalIpAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            throw new Exception("No network adapters with an IPv4 address in the system!");
        }

        public static AppSettings FromFile(string jsonSettingsPath)
        {
            // Check if configuration json exists
            if (!File.Exists(jsonSettingsPath))
                Default.SaveAs(jsonSettingsPath);

            // Load configuration from json
            var settingsJson = File.ReadAllText(jsonSettingsPath);
            var appSettings = JsonConvert.DeserializeObject<AppSettings>(settingsJson,new StringEnumConverter());

            return appSettings;
        }

        public static void SaveAs(this AppSettings settings, string jsonSettingsPath)
        {
            var json = JsonConvert.SerializeObject(settings, Formatting.Indented, new StringEnumConverter());
            File.WriteAllText(jsonSettingsPath, json);
        }

        public static AppSettings Default =>
            new()
            {
                SelectedPlaylist = "Eumel",
                ItunesLibrary = @$"{Environment.GetEnvironmentVariable("USERPROFILE")}\Music\iTunes\iTunes Music Library.xml",
                SongsPath = @$"{Environment.GetEnvironmentVariable("USERPROFILE")}\Music\iTunes\iTunes Media\Music",
                RestEndpoint = $"https://{GetLocalIpAddress()}:443",
                SyslogServer = GetLocalIpAddress(),
                ClientLogLevel = Constants.EumelLogLevel.Verbose,
                LoggerSettings = new LoggerSettings()
                {
                    ServerLogLevel = Constants.EumelLogLevel.Verbose.ToString(),
                    UseDebug = false,
                    UseConsole = true,
                    UseExtendedDebug = false,
                    Filelog = null,
                    Syslog = new SyslogSettings() { EnableSyslogLogging = true, UseUdp = true, SysLogServerIp = GetLocalIpAddress() }
                },
                ImplementationSettings = new ImplementationSettings()
                {
                    ISongsProviderService = "FileSystemMp3Searcher",
                    IDjPlaylistManager = "SimplePlaylistManager"
                }
            };
    }
}