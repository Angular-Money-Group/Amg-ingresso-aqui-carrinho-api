using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace Amg_ingressos_aqui_carrinho_api.Model.Pagbank
{
    public class Phone
    {
        public Phone()
        {
            Country = string.Empty;
            Area = string.Empty;
            Number = string.Empty;
            Type = string.Empty;
        }

        [JsonProperty("country")]
        [JsonPropertyName("country")]
        public string Country { get; set; }

        [JsonProperty("area")]
        [JsonPropertyName("area")]
        public string Area { get; set; }

        [JsonProperty("number")]
        [JsonPropertyName("number")]
        public string Number { get; set; }

        [JsonProperty("type")]
        [JsonPropertyName("type")]
        public string Type { get; set; }
    }
}