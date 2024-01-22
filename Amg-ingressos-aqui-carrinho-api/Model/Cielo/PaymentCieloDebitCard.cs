using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace Amg_ingressos_aqui_carrinho_api.Model.Cielo.DebitCard
{
    public class PaymentCieloDebitCard
    {
        public PaymentCieloDebitCard()
        {
            MerchantOrderId = string.Empty;
            Customer = new Customer();
            Payment = new Payment();
        }

        [JsonProperty("MerchantOrderId")]
        [JsonPropertyName("MerchantOrderId")]
        public string MerchantOrderId { get; set; }

        [JsonProperty("Customer")]
        [JsonPropertyName("Customer")]
        public Customer Customer { get; set; }

        [JsonProperty("Payment")]
        [JsonPropertyName("Payment")]
        public Payment Payment { get; set; }
    }
}