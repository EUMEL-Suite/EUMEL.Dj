namespace Eumel.Dj.Core.Models
{
    public interface IImplementationSettings
    {
        // ReSharper disable InconsistentNaming
        string ISongsProviderService { get; set; }
        string IDjPlaylistManager { get; set; }
    }
}