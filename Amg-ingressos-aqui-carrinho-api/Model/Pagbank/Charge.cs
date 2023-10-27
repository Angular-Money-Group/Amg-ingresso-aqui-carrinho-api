using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Amg_ingressos_aqui_carrinho_api.Model.Pagbank
{
    public class Charge
    {
        public string reference_id { get; set; }
        public string description { get; set; }
        public Amount amount { get; set; }
        public PaymentMethod payment_method { get; set; }
        public List<string> notification_urls { get; set; }
    }
}