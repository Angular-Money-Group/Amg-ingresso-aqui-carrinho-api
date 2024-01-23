using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace Amg_ingressos_aqui_carrinho_api.Model.Cielo.CreditCard
{
    public class PaymentCieloCreditCard
    {
        public PaymentCieloCreditCard()
        {
            MerchantOrderId = string.Empty;
            Payment = new Payment();
        }

        [JsonProperty("MerchantOrderId")]
        [JsonPropertyName("MerchantOrderId")]
        public string MerchantOrderId { get; set; }

        [JsonProperty("Payment")]
        [JsonPropertyName("Payment")]
        public Payment Payment { get; set; }
    }
}