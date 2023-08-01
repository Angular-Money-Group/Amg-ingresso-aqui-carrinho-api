using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Amg_ingressos_aqui_carrinho_api.Model.Cielo.DebitCard
{
    public class PaymentCieloDebitCard
    {
        public string MerchantOrderId { get; set; }
        public Customer Customer { get; set; }
        public Payment Payment { get; set; }
    }
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Customer
    {
        public string Name { get; set; }
    }

    public class DebitCard
    {
        public string CardNumber { get; set; }
        public string Holder { get; set; }
        public string ExpirationDate { get; set; }
        public string SecurityCode { get; set; }
        public string Brand { get; set; }
    }

    public class Payment
    {
        public string Type { get; set; }
        public int Amount { get; set; }
        public string Provider { get; set; }
        public string ReturnUrl { get; set; }
        public DebitCard DebitCard { get; set; }
    }


}