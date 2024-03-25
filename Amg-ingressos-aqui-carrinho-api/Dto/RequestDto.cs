using System.Text.Json.Serialization;
using Amg_ingressos_aqui_carrinho_api.Model.Pagbank;
using Newtonsoft.Json;
namespace Amg_ingressos_aqui_carrinho_api.Dto
{
    public class RequestDto
    {
        public RequestDto()
        {
            Customer = new Customer();
            PaymentMethod = new PaymentMethod();
            Amount = new Amount();
            BillingAddress = new Address();
            ShippingAddress = new Address();
        }

        [JsonProperty("customer")]
        [JsonPropertyName("customer")]
        public Customer Customer { get; set; }

        [JsonProperty("paymentMethod")]
        [JsonPropertyName("paymentMethod")]
        public PaymentMethod PaymentMethod { get; set; }

        [JsonProperty("amount")]
        [JsonPropertyName("amount")]
        public Amount Amount { get; set; }

        [JsonProperty("billingAddress")]
        [JsonPropertyName("billingAddress")]
        public Address BillingAddress { get; set; }

        [JsonProperty("shippingAddress")]
        [JsonPropertyName("shippingAddress")]
        public Address ShippingAddress { get; set; }

        [JsonProperty("dataOnly")]
        [JsonPropertyName("dataOnly")]
        public bool DataOnly { get; set; }

        public RequestDto TransactionToRequest(Model.Transaction transaction, Model.User user)
        {
            var totalValue = transaction.TotalValue.ToString().Contains('.') ? transaction.TotalValue.ToString().Replace(".", string.Empty).Replace(",", string.Empty) : String.Format("{0:0.00}", transaction.TotalValue).Replace(".", string.Empty).Replace(",", string.Empty);

            RequestDto request = new()
            {
                Customer = new Customer()
                {
                    Email = user.Contact.Email ?? string.Empty,
                    Name = user.Name,
                    Phones = new List<Phone>() {
                        new() {
                            Area = user.Contact?.PhoneNumber?[..2] ?? string.Empty,
                            Country = "55",
                            Number = user.Contact?.PhoneNumber?[2..user.Contact.PhoneNumber.Length ].Replace("-", string.Empty) ?? string.Empty,
                            Type = "MOBILE"
                        }
                    }
                },
                PaymentMethod = new PaymentMethod()
                {

                    Card = new Card()
                    {
                        ExpMonth = transaction?.PaymentMethod?.ExpirationDate?.Split("/")[0] ?? string.Empty,
                        ExpYear = transaction?.PaymentMethod?.ExpirationDate?.Split("/")[1] ?? string.Empty,
                        Holder = new Holder()
                        {
                            Name = transaction?.PaymentMethod.Holder ?? string.Empty,
                        },
                        Number = transaction?.PaymentMethod?.CardNumber?.Trim() ?? string.Empty
                    },
                    Installments = transaction?.PaymentMethod.Installments ?? 1,
                    Type = transaction.PaymentMethod.TypePayment.Equals(transaction.PaymentMethod.TypePayment) ? "CREDIT_CARD" : "DEBIT_CARD",
                },
                Amount = new Amount()
                {
                    Value = Convert.ToInt32(totalValue),
                    Currency = "BRL"
                },
                BillingAddress = new Address()
                {
                    Street = user.Address.AddressDescription ?? string.Empty,
                    Number = user.Address.AddressDescription ?? string.Empty,
                    Complement = user.Address.Complement == string.Empty ? null : user.Address.Complement,
                    RegionCode = user.Address.Neighborhood ?? string.Empty,
                    Country = "BRA",
                    City = user.Address.City ?? string.Empty,
                    PostalCode = user.Address.Cep ?? string.Empty,
                },
                ShippingAddress = new Address()
                {
                    Street = user.Address.AddressDescription?? string.Empty,
                    Number = user.Address.Number ?? string.Empty,
                    Complement = user.Address.Complement == string.Empty ? null : user.Address.Complement,
                    RegionCode = user.Address.State ?? string.Empty,
                    Country = "BRA",
                    City = user.Address.City ?? string.Empty,
                    PostalCode = user.Address.Cep ?? string.Empty,
                },
                DataOnly = false
            };

            return request;
        }
    }
}