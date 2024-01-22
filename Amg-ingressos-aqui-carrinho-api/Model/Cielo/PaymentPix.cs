namespace Amg_ingressos_aqui_carrinho_api.Model.Cielo
{
    public class PaymentPix
    {
        public PaymentPix()
        {
            QrCodeBase64Image = string.Empty;
            QrCodeString = string.Empty;
            Tid = string.Empty;
            ProofOfSale = string.Empty;
            SentOrderId = string.Empty;
            Provider = string.Empty;
            ReturnMessage = string.Empty;
            ReturnCode = string.Empty;
            PaymentId = string.Empty;
            Type = string.Empty;
            Currency = string.Empty;
            Country = string.Empty;
            Links = new List<Link>();
        }

        public string QrCodeBase64Image { get; set; }
        public string QrCodeString { get; set; }
        public string Tid { get; set; }
        public string ProofOfSale { get; set; }
        public string SentOrderId { get; set; }
        public double Amount { get; set; }
        public DateTime ReceivedDate { get; set; }
        public string Provider { get; set; }
        public int Status { get; set; }
        public bool IsSplitted { get; set; }
        public string ReturnMessage { get; set; }
        public string ReturnCode { get; set; }
        public string PaymentId { get; set; }
        public string Type { get; set; }
        public string Currency { get; set; }
        public string Country { get; set; }
        public List<Link> Links { get; set; }
    }
}