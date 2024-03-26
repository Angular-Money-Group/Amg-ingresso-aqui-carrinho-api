using Amg_ingressos_aqui_carrinho_api.Enum;

namespace Amg_ingressos_aqui_carrinho_api.Model
{
    public class PaymentPixDetails
    {
        public PaymentPixDetails()
        {
            Paymentid = string.Empty;
            Type = string.Empty;
            AcquirerTransactionId = string.Empty;
            ProofOfSale = string.Empty;
            QrcodeBase64Image = string.Empty;
            QrCodeString = string.Empty;
            ReceivedDate = string.Empty;
        }

        public string Paymentid { get; set; }
        public string Type { get; set; }
        public string AcquirerTransactionId { get; set; }
        public string ProofOfSale { get; set; }
        public string QrcodeBase64Image { get; set; }
        public string QrCodeString { get; set; }
        public long Amount { get; set; }
        public string ReceivedDate { get; set; }
        public int Status { get; set; }
        public StatusCallbackCielo ReturnCode { get; set; }
    }
}