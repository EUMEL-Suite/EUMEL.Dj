namespace Eumel.Dj.WebServer.Logging
{
    public interface IFilelogSettings
    {
        bool EnableFileLogging { get; set; }
        string FilePath { get; set; }
        string MinimumLevel { get; set; }
        string OutputTemplate { get; set; }
        int RetainedFileCountLimit { get; set; }
        int FileSizeLimitBytes { get; set; }
        bool Async { get; set; }
        bool RollOnFileSizeLimit { get; set; }
    }
}