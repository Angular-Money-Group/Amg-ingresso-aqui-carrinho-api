using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace Amg_ingressos_aqui_carrinho_api.Model.Cielo
{
    public class Customer
    {
        public Customer()
        {
            Name = string.Empty;
            Identity = string.Empty;
            Address = new Address();
            IdentityType = string.Empty;
        }

        [JsonProperty("Name")]
        [JsonPropertyName("Name")]
        public string Name { get; set; }

        [JsonProperty("Identity")]
        [JsonPropertyName("Identity")]
        public string Identity { get; set; }

        [JsonProperty("Address")]
        [JsonPropertyName("Address")]
        public Address Address { get; set; }

        [JsonProperty("IdentityType")]
        [JsonPropertyName("IdentityType")]
        public string IdentityType { get; set; }
    }
}