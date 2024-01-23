using System.Text.Json.Serialization;

namespace Amg_ingressos_aqui_carrinho_api.Model
{
    public class PaymentMethodPix
    {
        public PaymentMethodPix()
        {
            MerchantOrderId = string.Empty;
            Customer = new Customer();
            Payment = new Payment();
        }

        [JsonPropertyName("merchantOrderId")]
        public string MerchantOrderId { get; set; }

        [JsonPropertyName("customer")]
        public Customer Customer { get; set; }

        [JsonPropertyName("payment")]
        public Payment Payment { get; set; }
    }
}