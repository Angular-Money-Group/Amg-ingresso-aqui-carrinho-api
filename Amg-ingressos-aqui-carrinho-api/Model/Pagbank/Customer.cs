using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Amg_ingressos_aqui_carrinho_api.Model.Pagbank
{
    public class Customer
    {
        public string name { get; set; }
        public string email { get; set; }
        public string tax_id { get; set; }
        public List<Phone> phones { get; set; }
    }
}