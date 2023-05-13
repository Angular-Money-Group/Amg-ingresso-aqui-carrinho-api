using MessagingToolkit.QRCode.Codec;
using static System.Net.Mime.MediaTypeNames;

namespace Amg_ingressos_aqui_carrinho_api.Utils
{
    public class QrCode{
        public void QrCode()
        {
            try
                {
                    QRCodeEncoder qrCodecEncoder = new QRCodeEncoder();
                    qrCodecEncoder.QRCodeBackgroundColor = System.Drawing.Color.White;
                    qrCodecEncoder.QRCodeForegroundColor = System.Drawing.Color.Black;
                    qrCodecEncoder.CharacterSet = "UTF-8";
                    qrCodecEncoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE;
                    qrCodecEncoder.QRCodeScale = 6;
                    qrCodecEncoder.QRCodeVersion = 0;
                    qrCodecEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.Q;
                    //Image imageQRCode;
                    //string a ser gerada
                    String dados = "dados";
                    var imageQRCode = qrCodecEncoder.Encode(dados);
                    //picQrCode.Image = imageQRCode;
                }
                catch(Exception ex)
                {
                    throw;
                }
        }
    }    
}