using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Amg_ingressos_aqui_carrinho_api.Model.Cielo.CreditCard
{
    public class PaymentCieloCreditCard
    {
        public string MerchantOrderId { get; set; }
        public Payment Payment { get; set; }
    }

    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class CreditCard
    {
        public string CardNumber { get; set; }
        public string Holder { get; set; }
        public string ExpirationDate { get; set; }
        public string SecurityCode { get; set; }
        public string Brand { get; set; }
        public string SaveCard { get; set; }
        public CardOnFile CardOnFile { get; set; }
    }

    public class Payment
    {
        public string Type { get; set; }
        public int Amount { get; set; }
        public int Installments { get; set; }
        public string SoftDescriptor { get; set; }
        public CreditCard CreditCard { get; set; }
    }

    public class CardOnFile
    {
         private string usage = "First";
         private string reason = "Recurring";

        public string Usage
        {
            get { return usage; }
            set { usage = value; }
        }

        public string Reason
        {
            get { return reason; }
            set { reason =value; }
        }

        public CardOnFile() {
            Usage = usage;
            Reason = reason;
        }
    }

}