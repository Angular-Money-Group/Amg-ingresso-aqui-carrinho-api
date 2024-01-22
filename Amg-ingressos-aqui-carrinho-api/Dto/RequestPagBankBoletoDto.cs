using Amg_ingressos_aqui_carrinho_api.Model;
using Amg_ingressos_aqui_carrinho_api.Model.Pagbank;
using Customer = Amg_ingressos_aqui_carrinho_api.Model.Pagbank.Customer;

namespace Amg_ingressos_aqui_carrinho_api.Dto.Pagbank
{
    public class RequestPagBankBoletoDto
    {
        public string reference_id { get; set; }
        public Customer customer { get; set; }
        public List<Item> items { get; set; }
        public Shipping shipping { get; set; }
        public List<string> notification_urls { get; set; }
        public List<Charge> charges { get; set; }

        public Request TransactionToRequest(Transaction transaction, User user)
        {
            RequestPagBankBoletoDto request = new RequestPagBankBoletoDto()
            {
                customer = new Customer()
                {
                    Email = user.Contact.Email,
                    Name = user.Name,
                    Phones = new List<Phone>(){
                        new Phone(){
                            Area=user.Contact.PhoneNumber.Substring(0,2),
                            Country="55",
                            Number=user.Contact.PhoneNumber.Substring(2,(user.Contact.PhoneNumber.Length-2)),
                            Type="MOBILE"
                        }
                    },
                    TaxId = user.DocumentId
                },
                items = new List<Item>(){
                    new Item(){
                        Name = "Ingresso",
                        Quantity = 1,
                        ReferenceId = transaction.Id,
                        UnitAmount = (int)transaction.TotalValue
                    }
                },
                reference_id = transaction.Id,
                charges = new List<Charge>(){
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
                                due_date = DateTime.Now.ToString("yyyy-MM-dd"),
                                InstructionLines = new InstructionLines(){
                                    Line1 = "Pagamento processado para DESC Fatura",
                                    Line2 = "via Pagseguro"
                                },
                                Holder = new Model.Pagbank.Holder(){
                                    Address = new Model.Pagbank.Address(){
                                        Country= "Brasil",
                                        Region= user.Address.State,
                                        RegionCode= user.Address.State,
                                        City= user.Address.City,
                                        PostalCode= user.Address.Cep,
                                        Street= user.Address.AddressDescription,
                                        Number= user.Address.Number,
                                        Locality= user.Address.Neighborhood
                                    },
                                    Email=user.Contact.Email,
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