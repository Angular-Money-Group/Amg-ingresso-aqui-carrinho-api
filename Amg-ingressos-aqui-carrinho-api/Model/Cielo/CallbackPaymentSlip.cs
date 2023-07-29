using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Amg_ingressos_aqui_carrinho_api.Model.Cielo.Callback.Slip
{
    public class CallbackPaymentSlip
    {
        public string MerchantOrderId { get; set; }
        public Customer Customer { get; set; }
        public Payment Payment { get; set; }
    }
    
    public class Address
    {
        public string Street { get; set; }
        public string Number { get; set; }
        public string Complement { get; set; }
        public string ZipCode { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string District { get; set; }
        public int AddressType { get; set; }
    }

    public class Customer
    {
        public string Name { get; set; }
        public string Identity { get; set; }
        public Address Address { get; set; }
    }

    public class Link
    {
        public string Method { get; set; }
        public string Rel { get; set; }
        public string Href { get; set; }
    }

    public class Payment
    {
        public string Instructions { get; set; }
        public string ExpirationDate { get; set; }
        public string Demonstrative { get; set; }
        public string Url { get; set; }
        public string BoletoNumber { get; set; }
        public string BarCodeNumber { get; set; }
        public string DigitableLine { get; set; }
        public string Assignor { get; set; }
        public string Address { get; set; }
        public string Identification { get; set; }
        public int Bank { get; set; }
        public int Amount { get; set; }
        public string ReceivedDate { get; set; }
        public string Provider { get; set; }
        public int Status { get; set; }
        public bool IsSplitted { get; set; }
        public string PaymentId { get; set; }
        public string Type { get; set; }
        public string Currency { get; set; }
        public string Country { get; set; }
        public List<Link> Links { get; set; }
    }
}