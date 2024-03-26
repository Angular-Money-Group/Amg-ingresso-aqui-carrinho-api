using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace Amg_ingressos_aqui_carrinho_api.Model.Pagbank
{
    public class Item
    {
        public Item()
        {
            ReferenceId = string.Empty;
            Name = string.Empty;
        }

        [JsonProperty("reference_id")]
        [JsonPropertyName("reference_id")]
        public string ReferenceId { get; set; }

        [JsonProperty("name")]
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonProperty("quantity")]
        [JsonPropertyName("quantity")]
        public int Quantity { get; set; }

        [JsonProperty("unit_amount")]
        [JsonPropertyName("unit_amount")]
        public int UnitAmount { get; set; }
    }
}