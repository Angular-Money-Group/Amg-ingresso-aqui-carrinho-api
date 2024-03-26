using Amg_ingressos_aqui_carrinho_api.Enum;

namespace Amg_ingressos_aqui_carrinho_api.Model.Cielo
{
    public class PaymentGetStatus
    {
        public PaymentGetStatus()
        {
            Tid = string.Empty;
            ProofOfSale = string.Empty;
            Type = string.Empty;
            Currency = string.Empty;
            Country = string.Empty;
            Provider = string.Empty;
            Links = new List<Link>();
        }

        public int Installments { get; set; }
        public string Tid { get; set; }
        public string ProofOfSale { get; set; }
        public Guid PaymentId { get; set; }
        public string Type { get; set; }
        public decimal Amount { get; set; }
        public DateTime ReceivedDate { get; set; }
        public string Currency { get; set; }
        public string Country { get; set; }
        public string Provider { get; set; }
        public StatusCallbackCielo Status { get; set; }
        public List<Link> Links { get; set; }
    }
}