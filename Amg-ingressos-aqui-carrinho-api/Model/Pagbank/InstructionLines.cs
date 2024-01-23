using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace Amg_ingressos_aqui_carrinho_api.Model.Pagbank
{
    public class InstructionLines
    {
        public InstructionLines()
        {
            Line1 = string.Empty;
            Line2 = string.Empty;
        }

        [JsonProperty("line_1")]
        [JsonPropertyName("line_1")]
        public string Line1 { get; set; }

        [JsonProperty("line_2")]
        [JsonPropertyName("line_2")]
        public string Line2 { get; set; }
    }
}