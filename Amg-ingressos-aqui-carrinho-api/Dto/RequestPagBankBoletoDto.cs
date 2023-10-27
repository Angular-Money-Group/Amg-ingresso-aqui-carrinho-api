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
                    email = user.Contact.Email,
                    name = user.Name,
                    phones = new List<Phone>(){
                        new Phone(){
                            area=user.Contact.PhoneNumber.Substring(0,2),
                            country="55",
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
                        payment_method= new Model.Pagbank.PaymentMethod(){
                            capture=true,
                            boleto = new Boleto(){
                                due_date = DateTime.Now.ToString("yyyy-MM-dd"),
                                instruction_lines = new InstructionLines(){
                                    line_1 = "Pagamento processado para DESC Fatura",
                                    line_2 = "via Pagseguro"
                                },
                                holder = new Model.Pagbank.Holder(){
                                    address = new Model.Pagbank.Address(){
                                        country= "Brasil",
                                        region= user.Address.State,
                                        region_code= user.Address.State,
                                        city= user.Address.City,
                                        postal_code= user.Address.Cep,
                                        street= user.Address.AddressDescription,
                                        number= user.Address.Number,
                                        locality= user.Address.Neighborhood
                                    },
                                    email=user.Contact.Email,
                                    name=user.Name,
                                    tax_id= user.DocumentId
                                }
                            },
                            installments= 1,
                            type="BOLETO"
                        }
                    }
                }
            };

            return new Request() { Data = System.Text.Json.JsonSerializer.Serialize(request) };
        }
    }
}