using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace Amg_ingressos_aqui_carrinho_api.Model.Pagbank
{
    public class Shipping
    {
        public Shipping()
        {
            Address = new Address();
        }

        [JsonProperty("address")]
        [JsonPropertyName("address")]
        public Address Address { get; set; }
    }
}