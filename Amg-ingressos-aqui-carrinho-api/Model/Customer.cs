using System.Text.Json.Serialization;

namespace Amg_ingressos_aqui_carrinho_api.Model
{
    public class Customer
    {
        public Customer()
        {
            Name = string.Empty;
            Identity = string.Empty;
            IdentityType = string.Empty;
        }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("identity")]
        public string Identity { get; set; }

        [JsonPropertyName("identityType")]
        public string IdentityType { get; set; }
    }
}