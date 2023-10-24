using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Amg_ingressos_aqui_carrinho_api.Model;

namespace Amg_ingressos_aqui_carrinho_api.Dto.Pagbank
{
    public class RequestPagBankDto
    {
        public string reference_id { get; set; }
        public Customer customer { get; set; }
        public List<Item> items { get; set; }
        
        [JsonIgnore]
        public List<QrCode> qr_codes { get; set; }
        
        [JsonIgnore]
        public Shipping shipping { get; set; }
        
        [JsonIgnore]
        public List<string> notification_urls { get; set; }
        public List<Charge> charges { get; set; }

        public Request TransactionToRequest(Transaction transaction, User user)
        {
            RequestPagBankDto request = new RequestPagBankDto()
            {
                customer = new Dto.Pagbank.Customer()
                {
                    email = user.Contact.Email,
                    name = user.Name,
                    phones = new List<Phone>(){
                        new Phone(){
                            //area=user.Contact.PhoneNumber.Substring(0,2),
                            //country="55",
                            number=user.Contact.PhoneNumber.Substring(2,(user.Contact.PhoneNumber.Length-2)),
                            type="MOBILE"
                        }
                    },
                    tax_id = user.DocumentId
                },
                items = new List<Item>(){
                    new Item(){
                        name = "Ingresso",
                        quantity = 1,
                        reference_id = transaction.Id,
                        unit_amount = (int)transaction.TotalValue
                    }
                },
                reference_id = transaction.Id,
                charges = new List<Charge>(){
                    new Charge(){
                        amount = new Amount(){
                            value= (int)transaction.TotalValue,
                            currency = "BRL"
                        },
                        reference_id = transaction.Id,
                        description="Ingressos",
                        notification_urls=new List<string>(),
                        payment_method= new PaymentMethod(){
                            capture=true,
                            card= new Card(){
                                exp_month=transaction.PaymentMethod.ExpirationDate.Split("/")[0],
                                exp_year=transaction.PaymentMethod.ExpirationDate.Split("/")[1],
                                holder=new Holder(){
                                    name=transaction.PaymentMethod.Holder,
                                },
                                number= "4111111111111111",//transaction.PaymentMethod.CardNumber.Trim(),
                                security_code=transaction.PaymentMethod.SecurityCode,
                                store=false
                            },
                            installments= 1,
                            type="CREDIT_CARD"
                        }
                    }
                }
            };

            return new Request() { Data = System.Text.Json.JsonSerializer.Serialize(request) };
        }
    }

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
        public string currency { get; set; }
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
        public string reference_id { get; set; }
        public string name { get; set; }
        public int quantity { get; set; }
        public int unit_amount { get; set; }
    }

    public class Phone
    {
        public string country { get; set; }
        public string area { get; set; }
        public string number { get; set; }
        public string type { get; set; }
    }

    public class QrCode
    {
        public Amount amount { get; set; }
    }

    public class Shipping
    {
        public Address address { get; set; }
    }
    public class Charge
    {
        public string reference_id { get; set; }
        public string description { get; set; }
        public Amount amount { get; set; }
        public PaymentMethod payment_method { get; set; }
        public List<string> notification_urls { get; set; }
    }
    public class PaymentMethod
    {
        public string type { get; set; }
        public int installments { get; set; }
        public bool capture { get; set; }
        public Card card { get; set; }
    }
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