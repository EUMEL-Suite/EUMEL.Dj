using System.Drawing;

namespace Eumel.Dj.WebServer.Models
{
    public interface IQrCodeService
    {
        Bitmap CreateInitCode(string restEndpoint);
    }
}