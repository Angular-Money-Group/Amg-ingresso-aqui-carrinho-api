using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Amg_ingressos_aqui_carrinho_api.Model.Pagbank
{
    public class QrCode
    {
        public Amount amount { get; set; }
        public DateTime expiration_date { get; set; }
    }
}