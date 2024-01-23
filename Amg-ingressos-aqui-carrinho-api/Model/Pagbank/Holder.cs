using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace Amg_ingressos_aqui_carrinho_api.Model.Pagbank
{
    public class Holder
    {
        public Holder()
        {
            Name = string.Empty;
            TaxId = string.Empty;
            Email = string.Empty;
            Address = new Address();
        }

        [JsonProperty("name")]
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonProperty("tax_id")]
        [JsonPropertyName("tax_id")]
        [System.Text.Json.Serialization.JsonIgnore]
        public string TaxId { get; set; }

        [JsonProperty("email")]
        [JsonPropertyName("email")]
        [System.Text.Json.Serialization.JsonIgnore]
        public string Email { get; set; }

        [JsonProperty("address")]
        [JsonPropertyName("address")]
        [System.Text.Json.Serialization.JsonIgnore]
        public Address Address { get; set; }
    }
}