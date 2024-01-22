using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Amg_ingressos_aqui_carrinho_api.Model.Pagbank.Pix
{
    public class CallbackPixPagBank
    {
        public CallbackPixPagBank()
        {
            id = string.Empty;
            ReferenceId = string.Empty;
            Customer = new Customer();
            Items = new List<Item>();
            Shipping = new Shipping();
            QrCodes = new List<QrCode>();
            NotificationUrls = new List<string>();
            Links = new List<Link>();
        }

        [JsonProperty("id")]
        [JsonPropertyName("id")]
        public string id { get; set; }

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

        [JsonProperty("qr_codes")]
        [JsonPropertyName("qr_codes")]
        public List<QrCode> QrCodes { get; set; }

        [JsonProperty("notification_urls")]
        [JsonPropertyName("notification_urls")]
        public List<string> NotificationUrls { get; set; }

        [JsonProperty("links")]
        [JsonPropertyName("links")]
        public List<Link> Links { get; set; }
    }
}