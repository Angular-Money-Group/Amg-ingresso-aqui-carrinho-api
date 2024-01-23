using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace Amg_ingressos_aqui_carrinho_api.Model.Pagbank
{
    public class PaymentMethod
    {
        public PaymentMethod()
        {
            Type = string.Empty;
            Card = new Card();
            Boleto = new Boleto();
            SoftDescriptor = string.Empty;
        }

        [JsonProperty("type")]
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonProperty("installments")]
        [JsonPropertyName("installments")]
        public int Installments { get; set; }

        [JsonProperty("capture")]
        [JsonPropertyName("capture")]
        public bool Capture { get; set; }

        [JsonProperty("card")]
        [JsonPropertyName("card")]
        public Card Card { get; set; }


        [JsonProperty("boleto")]
        [JsonPropertyName("boleto")]
        [Newtonsoft.Json.JsonIgnore]
        public Boleto Boleto { get; set; }

        [JsonProperty("soft_descriptor")]
        [JsonPropertyName("soft_descriptor")]
        public string SoftDescriptor { get; set; }
    }
}