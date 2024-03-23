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
            SecurityCode = null;
            Holder = new Holder();
            Encrypted = null;
            Brand = null;
            FirstDigits = null;
            LastDigits = null;
        }

        [JsonProperty("number")]
        [JsonPropertyName("number")]
        public string Number { get; set; }

        [JsonProperty("expMonth")]
        [JsonPropertyName("expMonth")]
        public string ExpMonth { get; set; }

        [JsonProperty("expYear")]
        [JsonPropertyName("expYear")]
        public string ExpYear { get; set; }

        [JsonProperty("security_code")]
        [JsonPropertyName("security_code")]
        public string? SecurityCode { get; set; }

        [JsonProperty("holder")]
        [JsonPropertyName("holder")]
        public Holder Holder { get; set; }

        [JsonProperty("store")]
        [JsonPropertyName("store")]
        public bool? Store { get; set; }

        [JsonProperty("encrypted")]
        [JsonPropertyName("encrypted")]
        public string? Encrypted { get; set; }

        [JsonProperty("brand")]
        [JsonPropertyName("brand")]
        public string? Brand { get; set; }

        [JsonProperty("first_digits")]
        [JsonPropertyName("first_digits")]
        public string? FirstDigits { get; set; }

        [JsonProperty("last_digits")]
        [JsonPropertyName("last_digits")]
        public string? LastDigits { get; set; }
    }
}