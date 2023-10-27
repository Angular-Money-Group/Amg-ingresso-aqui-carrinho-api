using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Amg_ingressos_aqui_carrinho_api.Model.Pagbank.Pix
{
    public class CallbackPixPagBank
    {
        public string id { get; set; }
        public string reference_id { get; set; }
        public DateTime created_at { get; set; }
        public Customer customer { get; set; }
        public List<Item> items { get; set; }
        public Shipping shipping { get; set; }
        public List<QrCode> qr_codes { get; set; }
        public List<string> notification_urls { get; set; }
        public List<Link> links { get; set; }
    }
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Address
    {
        public string street { get; set; }
        public string number { get; set; }
        public string complement { get; set; }
        public string locality { get; set; }
        public string city { get; set; }
        public string region_code { get; set; }
        public string country { get; set; }
        public string postal_code { get; set; }
    }

    public class Amount
    {
        public int value { get; set; }
    }

    public class Customer
    {
        public string name { get; set; }
        public string email { get; set; }
        public string tax_id { get; set; }
        public List<Phone> phones { get; set; }
    }

    public class Item
    {
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

    public class Phone
    {
        public string type { get; set; }
        public string country { get; set; }
        public string area { get; set; }
        public string number { get; set; }
    }

    public class QrCode
    {
        public string id { get; set; }
        public DateTime expiration_date { get; set; }
        public Amount amount { get; set; }
        public string text { get; set; }
        public List<string> arrangements { get; set; }
        public List<Link> links { get; set; }
    }

    public class Shipping
    {
        public Address address { get; set; }
    }


}