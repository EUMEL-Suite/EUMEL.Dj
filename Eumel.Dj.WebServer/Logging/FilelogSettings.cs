﻿namespace Eumel.Dj.WebServer.Logging
{
    public class FilelogSettings : IFilelogSettings
    {
        public bool EnableFileLogging { get; set; }
        public string FilePath { get; set; }
        public string MinimumLevel { get; set; }
        public string OutputTemplate { get; set; }
        public int RetainedFileCountLimit { get; set; }
        public int FileSizeLimitBytes { get; set; }
        public bool Async { get; set; }
        public bool RollOnFileSizeLimit { get; set; }
    }
}