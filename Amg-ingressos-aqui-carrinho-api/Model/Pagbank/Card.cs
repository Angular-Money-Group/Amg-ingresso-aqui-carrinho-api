using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Amg_ingressos_aqui_carrinho_api.Model.Pagbank
{
    public class Card
    {
        public string number { get; set; }
        public string exp_month { get; set; }
        public string exp_year { get; set; }
        public string security_code { get; set; }
        public Holder holder { get; set; }
        public bool store { get; set; }
    }
}