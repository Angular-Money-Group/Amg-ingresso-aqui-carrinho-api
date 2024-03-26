using Amg_ingressos_aqui_carrinho_api.Model;
using Newtonsoft.Json;

namespace Amg_ingressos_aqui_carrinho_api.Dto
{
    public class TicketEventDataDto
    {
        public TicketEventDataDto()
        {
            Lot = new Lot();
            Variant = new Variant();
            Event = new Event();
            Id = string.Empty;
            IdLot = string.Empty;
            IdUser = string.Empty;
            QrCode = string.Empty;
            Position = string.Empty;
        }
        public Lot Lot { get; set; }
        public Variant Variant { get; set; }
        public Event Event { get; set; }
        public string Id { get; set; }
        public string IdLot { get; set; }
        public string IdUser { get; set; }
        
        [JsonProperty("position")]
        public object Position { get; set; }
        public double Value { get; set; }
        public bool IsSold { get; set; }
        public bool ReqDocs { get; set; }
        public string QrCode { get; set; }
    }
}