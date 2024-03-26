using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace Amg_ingressos_aqui_carrinho_api.Model.Pagbank.Callback.Boleto
{
    public class CallbackBoletoPagBank
    {
        public CallbackBoletoPagBank()
        {
            Id = string.Empty;
            ReferenceId = string.Empty;
            Customer = new Customer();
            Items = new List<Item>();
            Shipping = new Shipping();
            Charges = new List<Charge>();
            NotificationUrls = new List<string>();
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

        [JsonProperty("shipping")]
        [JsonPropertyName("shipping")]
        public Shipping Shipping { get; set; }

        [JsonProperty("charges")]
        [JsonPropertyName("charges")]
        public List<Charge> Charges { get; set; }

        [JsonProperty("notification_urls")]
        [JsonPropertyName("notification_urls")]
        public List<string> NotificationUrls { get; set; }

        [JsonProperty("links")]
        [JsonPropertyName("links")]
        public List<Link> Links { get; set; }

    }
}