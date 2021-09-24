using Eumel.Dj.Core;
using Eumel.Dj.Ui.Extensions.Apple;
using Eumel.Dj.Ui.Extensions.FileSystem;
using Eumel.Dj.Ui.Extensions.PlaylistManager;
using StructureMap;

namespace Eumel.Dj.Ui.Extensions
{
    public class ExtensionsServicesRegistry : Registry
    {
        public ExtensionsServicesRegistry()
        {
            _ = For<ISongsProviderService>().Use<ItunesProviderService>().Singleton();
            _ = For<ISongsProviderService>().Use<FileSystemMp3Searcher>().Singleton();

            _ = For<IDjPlaylistManager>().Use<SimplePlaylistManager>().Singleton();
        }
    }
}