using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace Amg_ingressos_aqui_carrinho_api.Model.Pagbank
{
    public class Boleto
    {
        public Boleto()
        {
            DueDate = string.Empty;
            InstructionLines = new InstructionLines();
            Holder = new Holder();
            Id = string.Empty;
            Barcode = string.Empty;
            FormattedBarcode = string.Empty;
        }

        [JsonProperty("due_date")]
        [JsonPropertyName("due_date")]
        public string DueDate { get; set; }

        [JsonProperty("instruction_lines")]
        [JsonPropertyName("instruction_lines")]
        public InstructionLines InstructionLines { get; set; }

        [JsonProperty("holder")]
        [JsonPropertyName("holder")]
        public Holder Holder { get; set; }

        [JsonProperty("id")]
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonProperty("barcode")]
        [JsonPropertyName("barcode")]
        public string Barcode { get; set; }

        [JsonProperty("formatted_barcode")]
        [JsonPropertyName("formatted_barcode")]
        public string FormattedBarcode { get; set; }
    }
}