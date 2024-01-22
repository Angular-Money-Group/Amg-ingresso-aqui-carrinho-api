using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace Amg_ingressos_aqui_carrinho_api.Model.Pagbank
{
    public class Summary
    {
        [JsonProperty("total")]
        [JsonPropertyName("total")]
        public int Total { get; set; }

        [JsonProperty("paid")]
        [JsonPropertyName("paid")]
        public int Paid { get; set; }

        [JsonProperty("refunded")]
        [JsonPropertyName("refunded")]
        public int Refunded { get; set; }
    }
}