using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace Amg_ingressos_aqui_carrinho_api.Model
{
    public class Address
    {
        public Address()
        {
            Cep = string.Empty;
            AddressDescription = string.Empty;
            Number = string.Empty;
            Neighborhood = string.Empty;
            Complement = string.Empty;
            ReferencePoint = string.Empty;
            City = string.Empty;
            State = string.Empty;
        }

        /// <summary>
        /// Cep da residencia 
        /// </summary>
        [JsonProperty("Cep")]
        [JsonPropertyName("cep")]
        public string? Cep { get; set; }

        /// <summary>
        /// Endereço da residencia 
        /// </summary>
        [JsonProperty("AddressDescription")]
        [JsonPropertyName("addressDescription")]
        public string? AddressDescription { get; set; }

        /// <summary>
        /// Número da residencia
        /// </summary>
        [JsonProperty("Number")]
        [JsonPropertyName("number")]
        public string? Number { get; set; }

        /// <summary> 
        /// Complemento 
        /// </summary>
        [JsonProperty("Complement")]
        [JsonPropertyName("complement")]
        public string? Complement { get; set; }

        /// <summary> 
        /// Ponto de referencia 
        /// </summary>
        [JsonProperty("ReferencePoint")]
        [JsonPropertyName("referencePoint")]
        public string? ReferencePoint { get; set; }

        /// <summary>
        /// Bairro de residencia 
        /// </summary>
        [JsonProperty("Neighborhood")]
        [JsonPropertyName("neighborhood")]
        public string? Neighborhood { get; set; }

        /// <summary> 
        /// Cidade de residencia 
        /// </summary>
        [JsonProperty("City")]
        [JsonPropertyName("city")]
        public string? City { get; set; }

        /// <summary>
        /// Estado de residencia 
        /// </summary>
        [JsonProperty("State")]
        [JsonPropertyName("state")]
        public string? State { get; set; }
    }
}
