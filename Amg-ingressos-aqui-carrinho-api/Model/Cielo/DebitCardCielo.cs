using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace Amg_ingressos_aqui_carrinho_api.Model.Cielo
{
    public class DebitCardCielo
    {
        public DebitCardCielo()
        {
            CardNumber = string.Empty;
            Holder = string.Empty;
            ExpirationDate = string.Empty;
            SecurityCode = string.Empty;
            Brand = string.Empty;
        }

        [JsonProperty("CardNumber")]
        [JsonPropertyName("CardNumber")]
        public string CardNumber { get; set; }

        [JsonProperty("Holder")]
        [JsonPropertyName("Holder")]
        public string Holder { get; set; }

        [JsonProperty("ExpirationDate")]
        [JsonPropertyName("ExpirationDate")]
        public string ExpirationDate { get; set; }

        [JsonProperty("SecurityCode")]
        [JsonPropertyName("SecurityCode")]
        public string SecurityCode { get; set; }

        [JsonProperty("Brand")]
        [JsonPropertyName("Brand")]
        public string Brand { get; set; }
    }
}