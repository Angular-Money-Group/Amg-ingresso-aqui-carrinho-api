using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace Amg_ingressos_aqui_carrinho_api.Model.Pagbank
{
    public class QrCode
    {
        public QrCode()
        {
            Id = string.Empty;
            Text = string.Empty;
            Arrangements = new List<string>();
            links = new List<Link>();
            Amount = new Amount();
        }

        [JsonProperty("amount")]
        [JsonPropertyName("amount")]
        public Amount Amount { get; set; }

        [JsonProperty("expiration_date")]
        [JsonPropertyName("expiration_date")]
        public DateTime ExpirationDate { get; set; }

        [JsonProperty("id")]
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonProperty("text")]
        [JsonPropertyName("text")]
        public string Text { get; set; }

        [JsonProperty("arrangements")]
        [JsonPropertyName("arrangements")]
        public List<string> Arrangements { get; set; }

        [JsonProperty("links")]
        [JsonPropertyName("links")]
        public List<Link> links { get; set; }
    }
}