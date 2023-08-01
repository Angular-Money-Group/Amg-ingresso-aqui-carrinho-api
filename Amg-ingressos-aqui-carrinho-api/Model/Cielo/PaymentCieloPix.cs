using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amg_ingressos_aqui_carrinho_api.Enum;

namespace Amg_ingressos_aqui_carrinho_api.Model.Cielo.Pix
{
    public class PaymentCieloPix
    {
        public string MerchantOrderId { get; set; }
        public CustomerPix Customer { get; set; }
        public PaymentPix Payment { get; set; }

    }
    public class CustomerPix
    {
        public string Name { get; set; }
        public string Identity { get; set; }
        public string IdentityType { get; set; }
    }

    public class PaymentPix
    {
        public string Type { get; set; }
        public int Amount { get; set; }
    }

}