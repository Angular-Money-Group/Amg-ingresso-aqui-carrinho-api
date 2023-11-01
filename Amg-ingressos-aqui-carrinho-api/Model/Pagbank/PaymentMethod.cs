using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Amg_ingressos_aqui_carrinho_api.Model.Pagbank
{
    public class PaymentMethod
    {
        public string type { get; set; }
        public int installments { get; set; }
        public bool capture { get; set; }
        public Card card { get; set; }
        
        [JsonIgnore]
        public Boleto boleto { get; set; }
    }
}