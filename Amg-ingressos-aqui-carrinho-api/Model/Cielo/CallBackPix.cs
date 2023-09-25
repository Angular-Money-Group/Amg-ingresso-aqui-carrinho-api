using Amg_ingressos_aqui_carrinho_api.Enum;

using System;
using System.Collections.Generic;

namespace Amg_ingressos_aqui_carrinho_api.Model.Cielo.Callback
{
    public class CustomerPix
    {
        public string Name { get; set; }
        public string Identity { get; set; }
        public string IdentityType { get; set; }
    }

    public class PaymentPix
    {
        public string QrCodeBase64Image { get; set; }
        public string QrCodeString { get; set; }
        public string Tid { get; set; }
        public string ProofOfSale { get; set; }
        public string SentOrderId { get; set; }
        public double Amount { get; set; }
        public DateTime ReceivedDate { get; set; }
        public string Provider { get; set; }
        public int Status { get; set; }
        public bool IsSplitted { get; set; }
        public string ReturnMessage { get; set; }
        public string ReturnCode { get; set; }
        public string PaymentId { get; set; }
        public string Type { get; set; }
        public string Currency { get; set; }
        public string Country { get; set; }
        public List<Link> Links { get; set; }
    }

    public class CallbackPix
    {
        public string MerchantOrderId { get; set; }
        public CustomerPix Customer { get; set; }
        public PaymentPix Payment { get; set; }
    }
}
