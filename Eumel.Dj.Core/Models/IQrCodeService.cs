using System.Drawing;

namespace Eumel.Dj.Core.Models
{
    public interface IQrCodeService
    {
        Bitmap CreateInitCode(string restEndpoint);
    }
}