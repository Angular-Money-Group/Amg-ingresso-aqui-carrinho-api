using Amg_ingressos_aqui_carrinho_api.Enum;

namespace Amg_ingressos_aqui_carrinho_api.Model.Cielo
{
    public class PaymentCallbackCreditCard
    {
        public PaymentCallbackCreditCard()
        {
            Tid = string.Empty;
            SoftDescriptor = string.Empty;
            ReturnMessage = string.Empty;
            ReceivedDate = string.Empty;
            PaymentId = string.Empty;
            Currency = string.Empty;
            Provider = string.Empty;
            Type = string.Empty;
            Country = string.Empty;
            Links = new List<Link>();
            CreditCard = new CreditCardCielo();
            NewCard = new CreditCardCielo();
        }

        public int ServiceTaxAmount { get; set; }
        public int Installments { get; set; }
        public int Interest { get; set; }
        public bool Capture { get; set; }
        public bool Authenticate { get; set; }
        public bool Recurrent { get; set; }
        public CreditCardCielo CreditCard { get; set; }
        public string Tid { get; set; }
        public string SoftDescriptor { get; set; }
        public string Provider { get; set; }
        public CreditCardCielo NewCard { get; set; }
        public bool IsQrCode { get; set; }
        public int Amount { get; set; }
        public string ReceivedDate { get; set; }
        public int Status { get; set; }
        public bool IsSplitted { get; set; }
        public string ReturnMessage { get; set; }
        public StatusCallbackCielo ReturnCode { get; set; }
        public string PaymentId { get; set; }
        public string Type { get; set; }
        public string Currency { get; set; }
        public string Country { get; set; }
        public List<Link> Links { get; set; }
    }
}