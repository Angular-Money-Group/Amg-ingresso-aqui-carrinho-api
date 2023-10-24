using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Amg_ingressos_aqui_carrinho_api.Model.Pagbank
{
    public class CallbackCreditCardPagBank
    {
         public string id { get; set; }
        public string reference_id { get; set; }
        public DateTime created_at { get; set; }
        public Customer customer { get; set; }
        public List<Item> items { get; set; }
        public List<Charge> charges { get; set; }
        public List<object> notification_urls { get; set; }
        public List<Link> links { get; set; }
    }

   // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Amount
    {
        public int value { get; set; }
        public string currency { get; set; }
        public Summary summary { get; set; }
    }

    public class Card
    {
        public string brand { get; set; }
        public string first_digits { get; set; }
        public string last_digits { get; set; }
        public string exp_month { get; set; }
        public string exp_year { get; set; }
        public Holder holder { get; set; }
        public bool store { get; set; }
    }

    public class Charge
    {
        public string id { get; set; }
        public string reference_id { get; set; }
        public string status { get; set; }
        public DateTime created_at { get; set; }
        public DateTime paid_at { get; set; }
        public string description { get; set; }
        public Amount amount { get; set; }
        public PaymentResponse payment_response { get; set; }
        public PaymentMethod payment_method { get; set; }
        public List<Link> links { get; set; }
    }

    public class Customer
    {
        public string name { get; set; }
        public string email { get; set; }
        public string tax_id { get; set; }
        public List<Phone> phones { get; set; }
    }

    public class Holder
    {
        public string name { get; set; }
    }

    public class Item
    {
        public string reference_id { get; set; }
        public string name { get; set; }
        public int quantity { get; set; }
        public int unit_amount { get; set; }
    }

    public class Link
    {
        public string rel { get; set; }
        public string href { get; set; }
        public string media { get; set; }
        public string type { get; set; }
    }

    public class PaymentMethod
    {
        public string type { get; set; }
        public int installments { get; set; }
        public bool capture { get; set; }
        public Card card { get; set; }
        public string soft_descriptor { get; set; }
    }

    public class PaymentResponse
    {
        public string code { get; set; }
        public string message { get; set; }
        public string reference { get; set; }
        public RawData raw_data { get; set; }
    }

    public class Phone
    {
        public string type { get; set; }
        public string country { get; set; }
        public string area { get; set; }
        public string number { get; set; }
    }

    public class RawData
    {
        public string authorization_code { get; set; }
        public string nsu { get; set; }
        public string reason_code { get; set; }
    }

    public class Summary
    {
        public int total { get; set; }
        public int paid { get; set; }
        public int refunded { get; set; }
    }


}