using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace Amg_ingressos_aqui_carrinho_api.Model.Pagbank
{
    public class Card
    {
        public Card()
        {
            Number = string.Empty;
            ExpMonth = string.Empty;
            ExpYear = string.Empty;
            SecurityCode = string.Empty;
            Holder = new Holder();
            Encrypted = string.Empty;
            Brand = string.Empty;
            FirstDigits = string.Empty;
            LastDigits = string.Empty;
        }

        [JsonProperty("number")]
        [JsonPropertyName("number")]
        [System.Text.Json.Serialization.JsonIgnore]
        public string Number { get; set; }

        [JsonProperty("exp_month")]
        [JsonPropertyName("exp_month")]
        [System.Text.Json.Serialization.JsonIgnore]
        public string ExpMonth { get; set; }

        [JsonProperty("exp_year")]
        [JsonPropertyName("exp_year")]
        [System.Text.Json.Serialization.JsonIgnore]
        public string ExpYear { get; set; }

        [JsonProperty("security_code")]
        [JsonPropertyName("security_code")]
        public string SecurityCode { get; set; }

        [JsonProperty("holder")]
        [JsonPropertyName("holder")]
        public Holder Holder { get; set; }

        [JsonProperty("store")]
        [JsonPropertyName("store")]
        public bool Store { get; set; }

        [JsonProperty("encrypted")]
        [JsonPropertyName("encrypted")]
        public string Encrypted { get; set; }

        [JsonProperty("brand")]
        [JsonPropertyName("brand")]
        public string Brand { get; set; }

        [JsonProperty("first_digits")]
        [JsonPropertyName("first_digits")]
        public string FirstDigits { get; set; }

        [JsonProperty("last_digits")]
        [JsonPropertyName("last_digits")]
        public string LastDigits { get; set; }
    }
}