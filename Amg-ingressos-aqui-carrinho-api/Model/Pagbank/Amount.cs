using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace Amg_ingressos_aqui_carrinho_api.Model.Pagbank
{
    public class Amount
    {
        public Amount()
        {
            Currency = string.Empty;
            Summary = null;
        }

        [JsonProperty("value")]
        [JsonPropertyName("value")]
        public int Value { get; set; }

        [JsonProperty("currency")]
        [JsonPropertyName("currency")]
        public string Currency { get; set; }

        [JsonProperty("summary")]
        [JsonPropertyName("summary")]
        public Summary? Summary { get; set; }
    }
}