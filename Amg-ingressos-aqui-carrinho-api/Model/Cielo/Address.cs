using System.Text.Json.Serialization;
using MongoDB.Bson.Serialization.Attributes;

namespace Amg_ingressos_aqui_carrinho_api.Model.Cielo
{
    public class Address
    {
        public Address()
        {
            Street = string.Empty;
            Number = string.Empty;
            Complement = string.Empty;
            State = string.Empty;
            District = string.Empty;
            ZipCode = string.Empty;
            City = string.Empty;
            Country = string.Empty;
        }

        [BsonElement("Street")]
        [JsonPropertyName("Street")]
        public string Street { get; set; }

        [BsonElement("Number")]
        [JsonPropertyName("Number")]
        public string Number { get; set; }

        [BsonElement("Complement")]
        [JsonPropertyName("Complement")]
        public string Complement { get; set; }

        [BsonElement("ZipCode")]
        [JsonPropertyName("ZipCode")]
        public string ZipCode { get; set; }

        [BsonElement("District")]
        [JsonPropertyName("District")]
        public string District { get; set; }

        [BsonElement("City")]
        [JsonPropertyName("City")]
        public string City { get; set; }

        [BsonElement("State")]
        [JsonPropertyName("State")]
        public string State { get; set; }

        [BsonElement("Country")]
        [JsonPropertyName("Country")]
        public string Country { get; set; }
    }
}