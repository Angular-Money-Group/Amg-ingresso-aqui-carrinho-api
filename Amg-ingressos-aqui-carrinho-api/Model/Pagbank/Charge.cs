using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace Amg_ingressos_aqui_carrinho_api.Model.Pagbank
{
    public class Charge
    {
        public Charge()
        {
            ReferenceId = string.Empty;
            Description = string.Empty;
            Amount = new Amount();
            PaymentMethod = new PaymentMethod();
            NotificationUrls = null;
            AuthenticationMethod = new AuthenticationMethod();
            id = null;
            Status = null;
            PaymentResponse = null;
            Links = null;
        }

        [JsonProperty("reference_id")]
        [JsonPropertyName("reference_id")]
        public string ReferenceId { get; set; }

        [JsonProperty("description")]
        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonProperty("amount")]
        [JsonPropertyName("amount")]
        public Amount Amount { get; set; }

        [JsonProperty("payment_method")]
        [JsonPropertyName("payment_method")]
        public PaymentMethod PaymentMethod { get; set; }

        [JsonProperty("notification_urls")]
        [JsonPropertyName("notification_urls")]
        [System.Text.Json.Serialization.JsonIgnore]
        public List<string>? NotificationUrls { get; set; }

        [JsonProperty("authentication_method")]
        [JsonPropertyName("authentication_method")]
        public AuthenticationMethod AuthenticationMethod { get; set; }

        [JsonProperty("id")]
        [JsonPropertyName("id")]
        public string? id { get; set; }

        [JsonProperty("status")]
        [JsonPropertyName("status")]
        public string? Status { get; set; }

        [JsonProperty("created_at")]
        [JsonPropertyName("created_at")]
        public DateTime? CreatedAt { get; set; }

        [JsonProperty("paid_at")]
        [JsonPropertyName("paid_at")]
        public DateTime? PaidAt { get; set; }

        [JsonProperty("payment_response")]
        [JsonPropertyName("payment_response")]
        public PaymentResponse? PaymentResponse { get; set; }

        [JsonProperty("links")]
        [JsonPropertyName("links")]
        public List<Link>? Links { get; set; }
    }
}