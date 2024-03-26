using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace Amg_ingressos_aqui_carrinho_api.Model.Cielo
{
    public class PaymentCieloSlip
    {
        public PaymentCieloSlip()
        {
            MerchantOrderId = string.Empty;
            Customer = new Customer();
            Payment = new Payment();
        }

        [JsonProperty("State")]
        [JsonPropertyName("state")]
        public string MerchantOrderId { get; set; }

        [JsonProperty("State")]
        [JsonPropertyName("state")]
        public Customer Customer { get; set; }

        [JsonProperty("State")]
        [JsonPropertyName("state")]
        public Payment Payment { get; set; }
    }
}