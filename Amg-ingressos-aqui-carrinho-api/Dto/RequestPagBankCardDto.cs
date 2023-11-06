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
                    email = user.Contact.Email,
                    name = user.Name,
                    phones = new List<Phone>() {
                        new Phone() {
                            area = user.Contact.PhoneNumber.Substring(0, 2),
                            country = "55",
                            number = user.Contact.PhoneNumber.Substring(2, (user.Contact.PhoneNumber.Length - 2)).Replace("-", string.Empty),
                            type = "MOBILE"
                        }
                    },
                    tax_id = user.DocumentId
                },
                items = new List<Item>() {
                    new Item() {
                        name = "Ingresso",
                        quantity = 1,
                        reference_id = transaction.Id,
                        unit_amount = Convert.ToInt32(totalValue)
                    }
                },
                reference_id = transaction.Id,
                charges = new List<Charge>(){
                    new Charge(){
                        amount = new Amount(){
                            value = Convert.ToInt32(totalValue),
                            currency = "BRL"
                        },
                        reference_id = transaction.Id,
                        description="Ingressos",
                        notification_urls=new List<string>(),
                        payment_method= new PaymentMethod(){
                            capture=true,
                            card= new Card(){
                                //exp_month=transaction.PaymentMethod.ExpirationDate.Split("/")[0],
                                //exp_year=transaction.PaymentMethod.ExpirationDate.Split("/")[1],
                                holder=new Holder(){
                                    name=transaction.PaymentMethod.Holder,
                                },
                                //number= transaction.PaymentMethod.CardNumber.Trim(),
                                security_code=transaction.PaymentMethod.SecurityCode,
                                store=false,
                                encrypted =transaction.PaymentMethod.EncryptedCard 
                            },
                            installments= 1,
                            type= transaction.PaymentMethod.TypePayment.Equals(TypePaymentEnum.CreditCard) ? "CREDIT_CARD" : "DEBIT_CARD",
                        },
                        authentication_meethod = new Authentication_method(){
                            id = "3DS_15CB7893-4D23-44FA-97B7-AC1BE516D418",
                            type = "THREEDS"
                        }
                    }
                }
            };

            return new Request() { Data = System.Text.Json.JsonSerializer.Serialize(request) };
        }
    }
}