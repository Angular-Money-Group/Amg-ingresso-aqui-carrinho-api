using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace Amg_ingressos_aqui_carrinho_api.Model.Pagbank
{
    public class PaymentResponse
    {
        public PaymentResponse()
        {
            Code = string.Empty;
            Message = string.Empty;
            Reference = string.Empty;
            RawData = new RawData();
        }

        [JsonProperty("code")]
        [JsonPropertyName("code")]
        public string Code { get; set; }

        [JsonProperty("message")]
        [JsonPropertyName("message")]
        public string Message { get; set; }

        [JsonProperty("reference")]
        [JsonPropertyName("reference")]
        public string Reference { get; set; }

        [JsonProperty("raw_data")]
        [JsonPropertyName("raw_data")]
        public RawData RawData { get; set; }
    }
}