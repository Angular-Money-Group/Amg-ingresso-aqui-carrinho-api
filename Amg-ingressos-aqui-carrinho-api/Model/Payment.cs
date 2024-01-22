using System.Text.Json.Serialization;

namespace Amg_ingressos_aqui_carrinho_api.Model
{
    public class Payment
    {
        public Payment()
        {
            Type = string.Empty;
        }

        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("amount")]
        public float Amount { get; set; }
    }
}