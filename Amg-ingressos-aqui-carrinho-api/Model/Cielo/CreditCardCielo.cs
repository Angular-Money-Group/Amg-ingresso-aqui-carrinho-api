using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace Amg_ingressos_aqui_carrinho_api.Model.Cielo
{
    public class CreditCardCielo
    {
        public CreditCardCielo()
        {
            CardNumber = string.Empty;
            Holder = string.Empty;
            ExpirationDate = string.Empty;
            SecurityCode = string.Empty;
            Brand = string.Empty;
            SaveCard = string.Empty;
            CardOnFile = new CardOnFile();
        }

        [JsonProperty("Name")]
        [JsonPropertyName("Name")]
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

        [JsonProperty("SaveCard")]
        [JsonPropertyName("SaveCard")]
        public string SaveCard { get; set; }

        [JsonProperty("CardOnFile")]
        [JsonPropertyName("CardOnFile")]
        public CardOnFile CardOnFile { get; set; }
    }
}