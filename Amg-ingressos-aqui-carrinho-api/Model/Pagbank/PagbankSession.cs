using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace Amg_ingressos_aqui_carrinho_api.Model.Pagbank
{
    public class PagbankSession
    {
        public PagbankSession()
        {
            Session = string.Empty;
            Expires_at = string.Empty;
        }

        [JsonProperty("session")]
        [JsonPropertyName("session")]
        public string Session { get; set; }

        [JsonProperty("expires_at")]
        [JsonPropertyName("expires_at")]
        public string Expires_at { get; set; }
    }
}