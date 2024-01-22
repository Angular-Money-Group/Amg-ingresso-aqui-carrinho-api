using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace Amg_ingressos_aqui_carrinho_api.Model.Pagbank
{
    public class Address
    {
        public Address()
        {
            Street = string.Empty;
            Number = string.Empty;
            Complement = string.Empty;
            Locality = string.Empty;
            City = string.Empty;
            RegionCode = string.Empty;
            Country = string.Empty;
            PostalCode = string.Empty;
            Region = string.Empty;
        }

        [JsonProperty("street")]
        [JsonPropertyName("street")]
        public string Street { get; set; }

        [JsonProperty("number")]
        [JsonPropertyName("number")]
        public string Number { get; set; }

        [JsonProperty("complement")]
        [JsonPropertyName("complement")]
        public string Complement { get; set; }

        [JsonProperty("locality")]
        [JsonPropertyName("locality")]
        public string Locality { get; set; }

        [JsonProperty("city")]
        [JsonPropertyName("city")]
        public string City { get; set; }

        [JsonProperty("region_code")]
        [JsonPropertyName("region_code")]
        public string RegionCode { get; set; }

        [JsonProperty("country")]
        [JsonPropertyName("country")]
        public string Country { get; set; }

        [JsonProperty("postal_code")]
        [JsonPropertyName("postal_code")]
        public string PostalCode { get; set; }

        [JsonProperty("region")]
        [JsonPropertyName("region")]
        public string Region { get; set; }

    }
}