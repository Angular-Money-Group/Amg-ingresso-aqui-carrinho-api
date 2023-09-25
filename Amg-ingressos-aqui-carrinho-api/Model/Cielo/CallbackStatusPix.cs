using Amg_ingressos_aqui_carrinho_api.Enum;

namespace Amg_ingressos_aqui_carrinho_api.Model.Cielo.Callback
{
    public class CustomerGetStatus
    {
        public string Name { get; set; }
        public string Identity { get; set; }
        public Dictionary<string, string> Address { get; set; }
    }

    public class LinkGetStatus
    {
        public string Method { get; set; }
        public string Rel { get; set; }
        public string Href { get; set; }
    }

    public class PaymentGetStatus
    {
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
        public List<LinkGetStatus> Links { get; set; }
    }

    public class PaymentTransactionGetStatus
    {
        public string MerchantOrderId { get; set; }
        public string AcquirerOrderId { get; set; }
        public CustomerGetStatus Customer { get; set; }
        public PaymentGetStatus Payment { get; set; }
    }
}
