using System.Text.Json.Serialization;
using Amg_ingressos_aqui_carrinho_api.Model;
using Amg_ingressos_aqui_carrinho_api.Model.Pagbank;
using Newtonsoft.Json;
using Customer = Amg_ingressos_aqui_carrinho_api.Model.Pagbank.Customer;

namespace Amg_ingressos_aqui_carrinho_api.Dto.Pagbank
{
    public class RequestPagBankBoletoDto
    {

        public RequestPagBankBoletoDto()
        {
            ReferenceId = string.Empty;
            Customer = new Customer();
            Items = new List<Item>();
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

        [JsonProperty("shipping")]
        [JsonPropertyName("shipping")]
        public Shipping Shipping { get; set; }

        [JsonProperty("notification_urls")]
        [JsonPropertyName("notification_urls")]
        public List<string> NotificationUrls { get; set; }

        [JsonProperty("charges")]
        [JsonPropertyName("charges")]
        public List<Charge> Charges { get; set; }

        public Request TransactionToRequest(Transaction transaction, User user)
        {
            RequestPagBankBoletoDto request = new RequestPagBankBoletoDto()
            {
                Customer = new Customer()
                {
                    Email = user.Contact.Email ?? string.Empty,
                    Name = user.Name,
                    Phones = new List<Phone>(){
                        new Phone(){
                            Area = user.Contact?.PhoneNumber?.Substring(0,2) ?? string.Empty,
                            Country ="55",
                            Number = user.Contact?.PhoneNumber?.Substring(2,(user.Contact.PhoneNumber.Length-2)) ??string.Empty,
                            Type = "MOBILE"
                        }
                    },
                    TaxId = user.DocumentId
                },
                Items = new List<Item>(){
                    new Item(){
                        Name = "Ingresso",
                        Quantity = 1,
                        ReferenceId = transaction.Id,
                        UnitAmount = (int)transaction.TotalValue
                    }
                },
                ReferenceId = transaction.Id,
                Charges = new List<Charge>(){
                    new Charge(){
                        Amount = new Amount(){
                            Value= (int)transaction.TotalValue,
                            Currency = "BRL"
                        },
                        ReferenceId = transaction.Id,
                        Description="Ingressos",
                        NotificationUrls=new List<string>(),
                        PaymentMethod= new Model.Pagbank.PaymentMethod(){
                            Capture=true,
                            Boleto = new Boleto(){
                                DueDate = DateTime.Now.ToString("yyyy-MM-dd"),
                                InstructionLines = new InstructionLines(){
                                    Line1 = "Pagamento processado para DESC Fatura",
                                    Line2 = "via Pagseguro"
                                },
                                Holder = new Model.Pagbank.Holder(){
                                    Address = new Model.Pagbank.Address(){
                                        Country= "Brasil",
                                        Region= user.Address.State ?? string.Empty,
                                        Region_Code= user.Address.State ?? string.Empty,
                                        City= user.Address.City ?? string.Empty,
                                        Postal_Code= user.Address.Cep ?? string.Empty,
                                        Street= user.Address.AddressDescription ?? string.Empty,
                                        Number= user.Address.Number ?? string.Empty,
                                        Locality= user.Address.Neighborhood ?? string.Empty
                                    },
                                    Email=user.Contact?.Email ?? string.Empty,
                                    Name=user.Name,
                                    TaxId= user.DocumentId
                                }
                            },
                            Installments= 1,
                            Type="BOLETO"
                        }
                    }
                }
            };

            return new Request() { Data = System.Text.Json.JsonSerializer.Serialize(request) };
        }
    }
}