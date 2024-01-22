using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace Amg_ingressos_aqui_carrinho_api.Model.Pagbank
{
    public class Customer
    {
        public Customer()
        {
            Name = string.Empty;
            Email = string.Empty;
            TaxId = string.Empty;
            Phones = new List<Phone>();
        }

        [JsonProperty("name")]
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonProperty("email")]
        [JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonProperty("tax_id")]
        [JsonPropertyName("tax_id")]
        public string TaxId { get; set; }

        [JsonProperty("phones")]
        [JsonPropertyName("phones")]
        public List<Phone> Phones { get; set; }
    }
}