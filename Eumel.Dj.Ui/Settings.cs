using System;

namespace Eumel.Dj.Ui
{
    public class Settings
    {
        public static Settings Default => new();

        public string ItunesLibrary { get; } =
            @$"{Environment.GetEnvironmentVariable("USERPROFILE")}\Music\iTunes\iTunes Music Library.xml";

        public string SelectedPlaylist { get; } = "Eumel";
    }
}