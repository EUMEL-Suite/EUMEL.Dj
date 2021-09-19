using Eumel.Dj.Ui.Core.Interfaces;
using Eumel.Dj.Ui.Core.ViewModels;
using StructureMap;

namespace Eumel.Dj.Ui.Bootstrapper
{
    public class ViewModelsRegistry : Registry
    {
        public ViewModelsRegistry()
        {
            _ = For<IShellViewModel>().Use<ShellViewModel>();
            _ = For<ILogOutputViewModel>().Use<LogOutputViewModel>();
            _ = For<ICurrentSongViewModel>().Use<CurrentSongViewModel>();
            _ = For<IChatViewModel>().Use<ChatViewModel>();
            _ = For<IStatusViewModel>().Use<StatusViewModel>();
            _ = For<IPlaylistViewModel>().Use<PlaylistViewModel>();
            _ = For<IPlayerViewModel>().Use<PlayerViewModel>();
        }
    }
}