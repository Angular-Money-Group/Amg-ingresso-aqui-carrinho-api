using System.Text.Json.Serialization;
using MongoDB.Bson.Serialization.Attributes;

namespace Amg_ingressos_aqui_carrinho_api.Model
{
    public class RemainingCourtesy
    {
        public RemainingCourtesy()
        {
            VariantName = string.Empty;
            VariantId = string.Empty;
        }

        /// <summary>
        /// Remaining courtesy quantity
        /// </summary>
        [BsonElement("Quantity")]
        [JsonPropertyName("quantity")]
        public int Quantity { get; set; }

        /// <summary>
        /// Courtesy history date
        /// </summary>
        [BsonElement("VariantName")]
        [JsonPropertyName("variantName")]
        public string VariantName { get; set; }

        /// <summary>
        /// Courtesy history date
        /// </summary>
        [BsonElement("VariantId")]
        [JsonPropertyName("variantId")]
        public string VariantId { get; set; }
    }
}