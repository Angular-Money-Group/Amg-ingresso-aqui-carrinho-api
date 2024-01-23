using System.Text.Json.Serialization;
using Amg_ingressos_aqui_carrinho_api.Model;
using Amg_ingressos_aqui_carrinho_api.Model.Pagbank;
using Newtonsoft.Json;
using Customer = Amg_ingressos_aqui_carrinho_api.Model.Pagbank.Customer;

namespace Amg_ingressos_aqui_carrinho_api.Dto
{
    public class RequestPagBankPixDto
    {
        public RequestPagBankPixDto()
        {
            ReferenceId = string.Empty;
            Customer = new Customer();
            Items = new List<Item>();
            QrCodes = new List<QrCode>();
            Shipping = new Shipping();
            NotificationUrls = new List<string>();
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
        public List<QrCode> QrCodes { get; set; }

        [JsonProperty("shipping")]
        [JsonPropertyName("shipping")]
        public Shipping Shipping { get; set; }

        [JsonProperty("notification_urls")]
        [JsonPropertyName("notification_urls")]
        public List<string> NotificationUrls { get; set; }

        public Request TransactionToRequest(Transaction transaction, User user)
        {
            RequestPagBankPixDto request = new RequestPagBankPixDto()
            {
                ReferenceId = transaction.Id,
                Customer = new Customer()
                {
                    Email = user.Contact.Email ?? string.Empty,
                    Name = user.Name,
                    Phones = new List<Phone>(){
                        new Phone(){
                            Area=user.Contact?.PhoneNumber?.Substring(0,2) ?? string.Empty,
                            Country="55",
                            Number=user.Contact?.PhoneNumber?.Substring(2,(user.Contact.PhoneNumber.Length-2)).Replace("-",string.Empty) ?? string.Empty,
                            Type="MOBILE"
                        }
                    },
                    TaxId = user.DocumentId
                },
                Items = new List<Item>(){
                    new Item(){
                        Name = "Ingresso",
                        Quantity = 1,
                        UnitAmount = (int)transaction.TotalValue
                    }
                },
                QrCodes = new List<QrCode>{
                    new QrCode(){
                        Amount = new Amount(){ Value = (int)transaction.TotalValue },
                        ExpirationDate= DateTime.Now.AddDays(3)
                    }
                },
                Shipping = new Shipping()
                {
                    Address = new Model.Pagbank.Address()
                    {
                        Country = "BRA",
                        Region = user.Address.State ?? "",
                        RegionCode = user.Address.State ?? "",
                        City = user.Address.City ?? "",
                        PostalCode = user.Address.Cep ?? "",
                        Street = user.Address.AddressDescription ?? user.Address.Neighborhood ?? "N/a",
                        Number = user.Address.Number ?? "",
                        Locality = user.Address.Neighborhood ?? ""
                    }
                }
            };

            return new Request() { Data = System.Text.Json.JsonSerializer.Serialize(request) };
        }
    }
}