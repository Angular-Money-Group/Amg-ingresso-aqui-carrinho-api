using System.Text.Json.Serialization;
using Amg_ingressos_aqui_carrinho_api.Enum;
using Amg_ingressos_aqui_carrinho_api.Model;
using Amg_ingressos_aqui_carrinho_api.Model.Pagbank;
using MongoDB.Bson;
using Newtonsoft.Json;
using Customer = Amg_ingressos_aqui_carrinho_api.Model.Pagbank.Customer;
using Holder = Amg_ingressos_aqui_carrinho_api.Model.Pagbank.Holder;
using PaymentMethod = Amg_ingressos_aqui_carrinho_api.Model.Pagbank.PaymentMethod;

namespace Amg_ingressos_aqui_carrinho_api.Dto.Pagbank
{
    public class RequestPagBankCardDto
    {

        public RequestPagBankCardDto()
        {
            ReferenceId = string.Empty;
            Customer = new Customer();
            Items = new List<Item>();
            QrCodes = new List<QrCode>();
            Shipping = new Shipping();
            NotificationUrls = new List<string>();
            Charges = new List<Charge>();
        }

        [JsonProperty("reference_id")]
        [JsonPropertyName("reference_id")]
        public string ReferenceId { get; set; }

        [JsonProperty("customer")]
        [JsonPropertyName("customer")]
        public Customer Customer { get; set; }

        [JsonProperty("items")]
        [JsonPropertyName("items")]
        public List<Item> Items { get; set; }

        [JsonProperty("qr_codes")]
        [JsonPropertyName("qr_codes")]
        [System.Text.Json.Serialization.JsonIgnore]
        public List<QrCode> QrCodes { get; set; }

        [System.Text.Json.Serialization.JsonIgnore]
        [JsonProperty("shipping")]
        [JsonPropertyName("shipping")]
        public Shipping Shipping { get; set; }

        [System.Text.Json.Serialization.JsonIgnore]
        [JsonProperty("notification_urls")]
        [JsonPropertyName("notification_urls")]
        public List<string> NotificationUrls { get; set; }

        [JsonProperty("charges")]
        [JsonPropertyName("charges")]
        public List<Charge> Charges { get; set; }


        public Request TransactionToRequest(Transaction transaction, User user)
        {
            var totalValue = transaction.TotalValue.ToString().Contains(".") ? transaction.TotalValue.ToString().Replace(".", string.Empty).Replace(",", string.Empty) : String.Format("{0:0.00}", transaction.TotalValue).Replace(".", string.Empty).Replace(",", string.Empty);

            RequestPagBankCardDto request = new RequestPagBankCardDto()
            {
                Customer = new Customer()
                {
                    Email = user.Contact.Email ?? string.Empty,
                    Name = user.Name,
                    Phones = new List<Phone>() {
                        new Phone() {
                            Area = user.Contact?.PhoneNumber?.Substring(0, 2) ?? string.Empty,
                            Country = "55",
                            Number = user.Contact?.PhoneNumber?.Substring(2, (user.Contact.PhoneNumber.Length - 2)).Replace("-", string.Empty) ?? string.Empty,
                            Type = "MOBILE"
                        }
                    },
                    TaxId = user.DocumentId
                },
                Items = new List<Item>() {
                    new Item() {
                        Name = "Ingresso",
                        Quantity = 1,
                        ReferenceId = transaction.Id,
                        UnitAmount = Convert.ToInt32(totalValue)
                    }
                },
                ReferenceId = transaction.Id,
                Charges = new List<Charge>(){
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
                                    Name = transaction.PaymentMethod.Holder ?? string.Empty,
                                },
                                //number= transaction.PaymentMethod.CardNumber.Trim(),
                                SecurityCode = transaction.PaymentMethod.SecurityCode ?? string.Empty,
                                Store = false,
                                Encrypted = transaction.PaymentMethod.EncryptedCard
                            },
                            Installments = 1,
                            Type = transaction.PaymentMethod.TypePayment.Equals(TypePayment.CreditCard) ? "CREDIT_CARD" : "DEBIT_CARD",
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