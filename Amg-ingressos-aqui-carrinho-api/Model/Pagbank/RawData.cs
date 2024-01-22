using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace Amg_ingressos_aqui_carrinho_api.Model.Pagbank
{
    public class RawData
    {
        public RawData()
        {
            AuthorizationCode = string.Empty;
            Nsu = string.Empty;
            ReasonCode = string.Empty;
        }

        [JsonProperty("authorization_code")]
        [JsonPropertyName("authorization_code")]

        public string AuthorizationCode { get; set; }

        [JsonProperty("nsu")]
        [JsonPropertyName("nsu")]
        public string Nsu { get; set; }

        [JsonProperty("reason_code")]
        [JsonPropertyName("reason_code")]
        public string ReasonCode { get; set; }
    }
}