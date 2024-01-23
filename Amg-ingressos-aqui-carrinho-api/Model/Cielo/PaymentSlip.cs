namespace Amg_ingressos_aqui_carrinho_api.Model.Cielo
{
    public class PaymentSlip
    {
        public PaymentSlip()
        {
            Instructions = string.Empty;
            Demonstrative = string.Empty;
            BoletoNumber = string.Empty;
            DigitableLine = string.Empty;
            Address = string.Empty;
            ReceivedDate = string.Empty;
            PaymentId = string.Empty;
            Currency = string.Empty;
            ExpirationDate = string.Empty;
            Url = string.Empty;
            BarCodeNumber = string.Empty;
            Assignor = string.Empty;
            Identification = string.Empty;
            Provider = string.Empty;
            Type = string.Empty;
            Country = string.Empty;
            Links = new List<Link>();
        }

        public string Instructions { get; set; }
        public string ExpirationDate { get; set; }
        public string Demonstrative { get; set; }
        public string Url { get; set; }
        public string BoletoNumber { get; set; }
        public string BarCodeNumber { get; set; }
        public string DigitableLine { get; set; }
        public string Assignor { get; set; }
        public string Address { get; set; }
        public string Identification { get; set; }
        public int Bank { get; set; }
        public int Amount { get; set; }
        public string ReceivedDate { get; set; }
        public string Provider { get; set; }
        public int Status { get; set; }
        public bool IsSplitted { get; set; }
        public string PaymentId { get; set; }
        public string Type { get; set; }
        public string Currency { get; set; }
        public string Country { get; set; }
        public List<Link> Links { get; set; }
    }
}