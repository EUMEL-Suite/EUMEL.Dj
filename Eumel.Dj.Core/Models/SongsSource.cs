namespace Eumel.Dj.Core.Models
{
    public class SongsSource
    {
        public string SourceName { get; }
        public int NumberOfSongs { get; }

        public SongsSource(string sourceName, int numberOfSongs)
        {
            SourceName = sourceName;
            NumberOfSongs = numberOfSongs;
        }
    }
}