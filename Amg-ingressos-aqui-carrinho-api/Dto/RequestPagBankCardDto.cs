using System.Text.Json.Serialization;
using Amg_ingressos_aqui_carrinho_api.Enum;
using Amg_ingressos_aqui_carrinho_api.Model;
using Amg_ingressos_aqui_carrinho_api.Model.Pagbank;
using Customer = Amg_ingressos_aqui_carrinho_api.Model.Pagbank.Customer;
using Holder = Amg_ingressos_aqui_carrinho_api.Model.Pagbank.Holder;
using PaymentMethod = Amg_ingressos_aqui_carrinho_api.Model.Pagbank.PaymentMethod;

namespace Amg_ingressos_aqui_carrinho_api.Dto.Pagbank
{
    public class RequestPagBankCardDto
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
            var totalValue= transaction.TotalValue.ToString().Contains(".") ? transaction.TotalValue.ToString().Replace(".",string.Empty).Replace(",",string.Empty) : String.Format("{0:0.00}", transaction.TotalValue).Replace(".",string.Empty).Replace(",",string.Empty);

            RequestPagBankCardDto request = new RequestPagBankCardDto()
            {
                customer = new Customer()
                {
                    Email = user.Contact.Email,
                    Name = user.Name,
                    Phones = new List<Phone>() {
                        new Phone() {
                            Area = user.Contact.PhoneNumber.Substring(0, 2),
                            Country = "55",
                            Number = user.Contact.PhoneNumber.Substring(2, (user.Contact.PhoneNumber.Length - 2)).Replace("-", string.Empty),
                            Type = "MOBILE"
                        }
                    },
                    TaxId = user.DocumentId
                },
                items = new List<Item>() {
                    new Item() {
                        Name = "Ingresso",
                        Quantity = 1,
                        ReferenceId = transaction.Id,
                        UnitAmount = Convert.ToInt32(totalValue)
                    }
                },
                reference_id = transaction.Id,
                charges = new List<Charge>(){
                    new Charge(){
                        Amount = new Amount(){
                            Value = Convert.ToInt32(totalValue),
                            Currency = "BRL"
                        },
                        ReferenceId = transaction.Id,
                        Description="Ingressos",
                        NotificationUrls=new List<string>(),
                        PaymentMethod= new PaymentMethod(){
                            Capture=true,
                            Card= new Card(){
                                //exp_month=transaction.PaymentMethod.ExpirationDate.Split("/")[0],
                                //exp_year=transaction.PaymentMethod.ExpirationDate.Split("/")[1],
                                Holder=new Holder(){
                                    Name=transaction.PaymentMethod.Holder,
                                },
                                //number= transaction.PaymentMethod.CardNumber.Trim(),
                                SecurityCode=transaction.PaymentMethod.SecurityCode,
                                Store=false,
                                Encrypted =transaction.PaymentMethod.EncryptedCard 
                            },
                            Installments= 1,
                            Type= transaction.PaymentMethod.TypePayment.Equals(TypePayment.CreditCard) ? "CREDIT_CARD" : "DEBIT_CARD",
                        },
                        AuthenticationMethod = new AuthenticationMethod(){
                            Id = "3DS_15CB7893-4D23-44FA-97B7-AC1BE516D418",
                            Type = "THREEDS"
                        }
                    }
                }
            };

            return new Request() { Data = System.Text.Json.JsonSerializer.Serialize(request) };
        }
    }
}