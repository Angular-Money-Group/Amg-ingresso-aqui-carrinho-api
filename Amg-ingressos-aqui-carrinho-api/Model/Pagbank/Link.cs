using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace Amg_ingressos_aqui_carrinho_api.Model.Pagbank
{
    public class Link
    {
        public Link()
        {
            Rel = string.Empty;
            Href = string.Empty;
            Media = string.Empty;
            Type = string.Empty;
        }

        [JsonProperty("rel")]
        [JsonPropertyName("rel")]
        public string Rel { get; set; }

        [JsonProperty("href")]
        [JsonPropertyName("href")]
        public string Href { get; set; }

        [JsonProperty("media")]
        [JsonPropertyName("media")]
        public string Media { get; set; }

        [JsonProperty("type")]
        [JsonPropertyName("type")]
        public string Type { get; set; }
    }
}