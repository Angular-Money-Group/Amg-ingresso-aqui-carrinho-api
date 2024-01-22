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
                    Email = user.Contact.Email,
                    Name = user.Name,
                    Phones = new List<Phone>(){
                        new Phone(){
                            Area=user.Contact.PhoneNumber.Substring(0,2),
                            Country="55",
                            Number=user.Contact.PhoneNumber.Substring(2,(user.Contact.PhoneNumber.Length-2)).Replace("-",string.Empty),
                            Type="MOBILE"
                        }
                    },
                    TaxId = user.DocumentId
                },
                items = new List<Item>(){
                    new Item(){
                        Name = "Ingresso",
                        Quantity = 1,
                        UnitAmount = (int)transaction.TotalValue
                    }
                },
                qr_codes = new List<QrCode>{
                    new QrCode(){
                        Amount = new Amount(){ Value = (int)transaction.TotalValue },
                        ExpirationDate= DateTime.Now.AddDays(3)
                    }
                },
                shipping= new Shipping(){
                    Address = new Model.Pagbank.Address(){
                        Country= "BRA",
                        Region= user.Address.State??"",
                        RegionCode= user.Address.State??"",
                        City= user.Address.City??"",
                        PostalCode= user.Address.Cep??"",
                        Street= user.Address.AddressDescription?? user.Address.Neighborhood ?? "N/a",
                        Number= user.Address.Number??"",
                        Locality= user.Address.Neighborhood??""
                    }
                }
            };

            return new Request() { Data = System.Text.Json.JsonSerializer.Serialize(request) };
        }
    }
}