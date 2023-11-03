using Amg_ingressos_aqui_carrinho_api.Model;
using Amg_ingressos_aqui_carrinho_api.Model.Pagbank;
using Customer = Amg_ingressos_aqui_carrinho_api.Model.Pagbank.Customer;

namespace Amg_ingressos_aqui_carrinho_api.Dto
{
    public class RequestPagBankPixDto
    {
        public string reference_id { get; set; }
        public Customer customer { get; set; }
        public List<Item> items { get; set; }
        public List<QrCode> qr_codes { get; set; }
        public Shipping shipping { get; set; }
        public List<string> notification_urls { get; set; }

        public Request TransactionToRequest(Transaction transaction, User user)
        {
            RequestPagBankPixDto request = new RequestPagBankPixDto()
            {
                reference_id = transaction.Id,
                customer = new Customer()
                {
                    email = user.Contact.Email,
                    name = user.Name,
                    phones = new List<Phone>(){
                        new Phone(){
                            area=user.Contact.PhoneNumber.Substring(0,2),
                            country="55",
                            number=user.Contact.PhoneNumber.Substring(2,(user.Contact.PhoneNumber.Length-2)).Replace("-",string.Empty),
                            type="MOBILE"
                        }
                    },
                    tax_id = user.DocumentId
                },
                items = new List<Item>(){
                    new Item(){
                        name = "Ingresso",
                        quantity = 1,
                        unit_amount = (int)transaction.TotalValue
                    }
                },
                qr_codes = new List<QrCode>{
                    new QrCode(){
                        amount = new Amount(){ value = (int)transaction.TotalValue },
                        expiration_date= DateTime.Now.AddDays(3)
                    }
                },
                shipping= new Shipping(){
                    address = new Model.Pagbank.Address(){
                        country= "BRA",
                        region= user.Address.State??"",
                        region_code= user.Address.State??"",
                        city= user.Address.City??"",
                        postal_code= user.Address.Cep??"",
                        street= user.Address.AddressDescription?? user.Address.Neighborhood ?? "N/a",
                        number= user.Address.Number??"",
                        locality= user.Address.Neighborhood??""
                    }
                }
            };

            return new Request() { Data = System.Text.Json.JsonSerializer.Serialize(request) };
        }
    }
}