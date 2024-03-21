using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace Amg_ingressos_aqui_carrinho_api.Model.Pagbank
{
    public class PagbankPublicKey
    {
        public PagbankPublicKey()
        {
            Public_key = string.Empty;
            Created_at = string.Empty;
        }

        [JsonProperty("public_key")]
        [JsonPropertyName("public_key")]
        public string Public_key { get; set; }

        [JsonProperty("created_at")]
        [JsonPropertyName("created_at")]
        public string Created_at { get; set; }
    }
}