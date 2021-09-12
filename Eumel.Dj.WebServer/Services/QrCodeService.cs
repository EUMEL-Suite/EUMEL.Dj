using System.Drawing;
using Eumel.Dj.WebServer.Models;
using QRCoder;

namespace Eumel.Dj.WebServer.Services
{
    public class QrCodeService : IQrCodeService
    {
        public Bitmap CreateInitCode(string restEndpoint)
        {
            var qrCodeGenerator = new QRCodeGenerator();
            var qrCodeData = qrCodeGenerator.CreateQrCode(new PayloadGenerator.Url(restEndpoint), QRCodeGenerator.ECCLevel.Q);
            var qrCode = new QRCode(qrCodeData);

            return qrCode.GetGraphic(5);
        }
    }
}