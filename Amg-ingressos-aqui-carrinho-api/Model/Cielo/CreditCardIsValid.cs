namespace Amg_ingressos_aqui_carrinho_api.Model.Cielo
{
    public class CreditCardIsValid
    {
        public CreditCardIsValid()
        {
            ReturnCode = string.Empty;
            ReturnMessage = string.Empty;
            IssuerTransactionId = string.Empty;

        }

        public bool Valid { get; set; }
        public string ReturnCode { get; set; }
        public string ReturnMessage { get; set; }
        public string IssuerTransactionId { get; set; }
    }
}