﻿using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using Eumel.Dj.Core.Logging;
using Newtonsoft.Json;

namespace Eumel.Dj.Core.Models
{
    public class AppSettings : IAppSettings
    {
        public string RestEndpoint { get; set; }
        public string SyslogServer { get; set; }
        public Constants.EumelLogLevel MinimumLogLevel { get; set; }
        public LoggerSettings LoggerSettings { get; set; }
    }

    public static class AppSettingsExtensions
    {
        public static string GetLocalIpAddress()
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
            var appSettings = JsonConvert.DeserializeObject<AppSettings>(settingsJson);

            return appSettings;
        }

        public static void SaveAs(this AppSettings settings, string jsonSettingsPath)
        {
            var json = JsonConvert.SerializeObject(settings, Formatting.Indented);
            File.WriteAllText(jsonSettingsPath, json);
        }

        public static AppSettings Default =>
            new()
            {
                RestEndpoint = $"https://{GetLocalIpAddress()}:443",
                SyslogServer = GetLocalIpAddress(),
                MinimumLogLevel = Constants.EumelLogLevel.Information,
                LoggerSettings = new LoggerSettings()
                {
                    DeviceName = Environment.MachineName,
                    Filelog = null,
                    Syslog = new SyslogSettings() { EnableSyslogLogging = true, UseUdp = true, SysLogServerIp = GetLocalIpAddress() }
                }
            };
    }
}