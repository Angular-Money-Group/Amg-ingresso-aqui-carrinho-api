using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace Amg_ingressos_aqui_carrinho_api.Model.Pagbank.Callback
{
    public class CallbackCreditCardPagBank
    {
        public CallbackCreditCardPagBank()
        {
            Id = string.Empty;
            ReferenceId = string.Empty;
            Customer = new Customer();
            Items = new List<Item>();
            Charges = new List<Charge>();
            NotificationUrls = new List<object>();
            Links = new List<Link>();
        }

        [JsonProperty("id")]
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonProperty("reference_id")]
        [JsonPropertyName("reference_id")]
        public string ReferenceId { get; set; }

        [JsonProperty("created_at")]
        [JsonPropertyName("created_at")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("customer")]
        [JsonPropertyName("customer")]
        public Customer Customer { get; set; }

        [JsonProperty("items")]
        [JsonPropertyName("items")]
        public List<Item> Items { get; set; }

        [JsonProperty("charges")]
        [JsonPropertyName("charges")]
        public List<Charge> Charges { get; set; }

        [JsonProperty("notification_urls")]
        [JsonPropertyName("notification_urls")]
        public List<object> NotificationUrls { get; set; }

        [JsonProperty("links")]
        [JsonPropertyName("links")]
        public List<Link> Links { get; set; }
    }
}