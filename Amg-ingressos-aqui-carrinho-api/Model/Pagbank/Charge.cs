using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Amg_ingressos_aqui_carrinho_api.Model.Pagbank
{
    public class Charge
    {
        public string reference_id { get; set; }
        public string description { get; set; }
        public Amount amount { get; set; }
        public PaymentMethod payment_method { get; set; }
        [JsonIgnore]
        public List<string> notification_urls { get; set; }
        public Authentication_method authentication_meethod { get; set; }
    }
}