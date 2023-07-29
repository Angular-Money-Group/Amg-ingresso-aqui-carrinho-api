
using Amg_ingressos_aqui_carrinho_api.Enum;

public class CallbackPix
    {
        public string PaymentId { get; set; }
        public string AcquirerTransactionId { get; set; }
        public string ProofOfSale { get; set; }
        public string QrcodeBase64Image { get; set; }
        public string QrCodeString { get; set; }
        public int Status { get; set; }
        public StatusCallbackCielo ReturnCode { get; set; }
        public string ReturnMessage { get; set; }
    }