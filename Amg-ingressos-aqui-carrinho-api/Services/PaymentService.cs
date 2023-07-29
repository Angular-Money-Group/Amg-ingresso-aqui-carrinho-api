using System.Text;
using Amg_ingressos_aqui_carrinho_api.Infra;
using Amg_ingressos_aqui_carrinho_api.Model;
using Amg_ingressos_aqui_carrinho_api.Model.Cielo.Callback;
using Amg_ingressos_aqui_carrinho_api.Model.Cielo.CreditCard;
using Amg_ingressos_aqui_carrinho_api.Model.Cielo.DebitCard;
using Amg_ingressos_aqui_carrinho_api.Services.Interfaces;
using Amg_ingressos_aqui_carrinho_api.Enum;
using Newtonsoft.Json;
using static System.Net.Mime.MediaTypeNames;
using Amg_ingressos_aqui_carrinho_api.Model.Cielo;

namespace Amg_ingressos_aqui_carrinho_api.Services
{
    public class PaymentService : IPaymentService
    {
        private MessageReturn _messageReturn;
        private HttpClient _HttpClient;
        public PaymentService(ICieloClient cieloClient)
        {
            _HttpClient = cieloClient.CreateClient();
            _messageReturn = new Model.MessageReturn();
        }
        public async Task<MessageReturn> Payment(Transaction transaction)
        {
            //PaymentCieloDebitCardAsync(transaction);
            //PaymentCieloCreditCardAsync(transaction);
            StringContent transactionJson;
            if (transaction.PaymentMethod.TypePayment == Enum.TypePaymentEnum.CreditCard)
                PaymentCieloCreditCardAsync(transaction);
            else if (transaction.PaymentMethod.TypePayment == Enum.TypePaymentEnum.DebitCard)
                PaymentCieloDebitCardAsync(transaction);
            else if (transaction.PaymentMethod.TypePayment == Enum.TypePaymentEnum.DebitCard)
                ;
            else if (transaction.PaymentMethod.TypePayment == Enum.TypePaymentEnum.PaymentSlip)
                PaymentCieloSlipAsync(transaction);
            else
                throw new Exception("Tipo nao mapeado");


            return _messageReturn;
        }

        private void PaymentCieloDebitCardAsync(Transaction transaction)
        {
            var transactionToJson = new PaymentCieloDebitCard()
            {
                MerchantOrderId = transaction.Id,
                Customer = new Model.Cielo.DebitCard.Customer() { Name = "usuario sobrenome" },
                Payment = new Model.Cielo.DebitCard.Payment()
                {
                    Amount = (int)((transaction.TotalValue + transaction.Tax) - transaction.Discount),
                    DebitCard = new DebitCard()
                    {
                        Brand = transaction.PaymentMethod.Brand,
                        CardNumber = transaction.PaymentMethod.CardNumber,
                        ExpirationDate = transaction.PaymentMethod.ExpirationDate,
                        Holder = transaction.PaymentMethod.Holder,
                        SecurityCode = transaction.PaymentMethod.SecurityCode
                    },
                    Type = transaction.PaymentMethod.TypePayment.ToString(),
                    Provider = "Simulado",
                    ReturnUrl = "retorno url"
                }
            };

            var transactionJson = new StringContent(System.Text.Json.JsonSerializer.Serialize(transactionToJson), Encoding.UTF8, Application.Json);
            var objJson = SendRequestAsync(transactionJson);
            var obj = JsonConvert.DeserializeObject<CallbackCreditCard>(objJson.ToString());
            switch (obj.Payment.ReturnCode)
            {
                case StatusCallbackCielo.SuccessfullyPerformedOperation:
                    _messageReturn.Data = Consts.StatusCallbackCielo.SuccessfullyPerformedOperation;
                    transaction.Status = StatusPaymentEnum.Aproved;
                    break;
                case StatusCallbackCielo.NotAllowed:
                    _messageReturn.Message = Consts.StatusCallbackCielo.NotAllowed;
                    transaction.Status = StatusPaymentEnum.ErrorPayment;
                    transaction.Details = Consts.StatusCallbackCielo.NotAllowed;
                    break;
                case StatusCallbackCielo.ExpiredCard:
                    _messageReturn.Message = Consts.StatusCallbackCielo.ExpiredCard;
                    transaction.Status = StatusPaymentEnum.ErrorPayment;
                    transaction.Details = Consts.StatusCallbackCielo.ExpiredCard;
                    break;
                case StatusCallbackCielo.BlockedCard:
                    _messageReturn.Message = Consts.StatusCallbackCielo.BlockedCard;
                    transaction.Status = StatusPaymentEnum.ErrorPayment;
                    transaction.Details = Consts.StatusCallbackCielo.BlockedCard;
                    break;
                case StatusCallbackCielo.TimeOut:
                    _messageReturn.Message = Consts.StatusCallbackCielo.TimeOut;
                    transaction.Status = StatusPaymentEnum.ErrorPayment;
                    transaction.Details = Consts.StatusCallbackCielo.TimeOut;
                    break;
                case StatusCallbackCielo.CanceledCard:
                    _messageReturn.Message = Consts.StatusCallbackCielo.CanceledCard;
                    transaction.Status = StatusPaymentEnum.ErrorPayment;
                    transaction.Details = Consts.StatusCallbackCielo.CanceledCard;
                    break;
                case StatusCallbackCielo.CreditCardProblems:
                    _messageReturn.Message = Consts.StatusCallbackCielo.CreditCardProblems;
                    transaction.Status = StatusPaymentEnum.ErrorPayment;
                    transaction.Details = Consts.StatusCallbackCielo.CreditCardProblems;
                    break;
                case StatusCallbackCielo.SuccessfullyPerformedOperation2:
                    _messageReturn.Data = Consts.StatusCallbackCielo.SuccessfullyPerformedOperation;
                    transaction.Status = StatusPaymentEnum.Aproved;
                    transaction.Details = Consts.StatusCallbackCielo.SuccessfullyPerformedOperation;
                    break;
            }
            transaction.PaymentIdService = obj.Payment.PaymentId;
        }
        private async Task PaymentCieloCreditCardAsync(Transaction transaction)
        {
            var transactionToJson = new PaymentCieloCreditCard()
            {
                MerchantOrderId = transaction.Id,
                Payment = new Model.Cielo.CreditCard.Payment()
                {
                    Amount = (int)((transaction.TotalValue + transaction.Tax) - transaction.Discount),
                    CreditCard = new Model.Cielo.CreditCard.CreditCard()
                    {
                        Brand = transaction.PaymentMethod.Brand,
                        CardNumber = transaction.PaymentMethod.CardNumber,
                        ExpirationDate = transaction.PaymentMethod.ExpirationDate,
                        Holder = transaction.PaymentMethod.Holder,
                        SecurityCode = transaction.PaymentMethod.SecurityCode
                    },
                    Type = transaction.PaymentMethod.TypePayment.ToString(),
                    Installments = 1,
                    SoftDescriptor = "Tiquetera"
                }
            };

            var transactionJson = new StringContent(System.Text.Json.JsonSerializer.Serialize(transactionToJson), Encoding.UTF8, Application.Json);
            var objJson = SendRequestAsync(transactionJson);
            var obj = JsonConvert.DeserializeObject<CallbackCreditCard>(objJson.ToString());
            switch (obj.Payment.ReturnCode)
            {
                case StatusCallbackCielo.SuccessfullyPerformedOperation:
                    _messageReturn.Data = Consts.StatusCallbackCielo.SuccessfullyPerformedOperation;
                    transaction.Status = StatusPaymentEnum.Aproved;
                    transaction.Details = Consts.StatusCallbackCielo.SuccessfullyPerformedOperation;
                    break;
                case StatusCallbackCielo.NotAllowed:
                    _messageReturn.Message = Consts.StatusCallbackCielo.NotAllowed;
                    transaction.Status = StatusPaymentEnum.ErrorPayment;
                    transaction.Details = Consts.StatusCallbackCielo.NotAllowed;
                    break;
                case StatusCallbackCielo.ExpiredCard:
                    _messageReturn.Message = Consts.StatusCallbackCielo.ExpiredCard;
                    transaction.Status = StatusPaymentEnum.ErrorPayment;
                    transaction.Details = Consts.StatusCallbackCielo.ExpiredCard;
                    break;
                case StatusCallbackCielo.BlockedCard:
                    _messageReturn.Message = Consts.StatusCallbackCielo.BlockedCard;
                    transaction.Status = StatusPaymentEnum.ErrorPayment;
                    transaction.Details = Consts.StatusCallbackCielo.BlockedCard;
                    break;
                case StatusCallbackCielo.TimeOut:
                    _messageReturn.Message = Consts.StatusCallbackCielo.TimeOut;
                    transaction.Status = StatusPaymentEnum.ErrorPayment;
                    transaction.Details = Consts.StatusCallbackCielo.TimeOut;
                    break;
                case StatusCallbackCielo.CanceledCard:
                    _messageReturn.Message = Consts.StatusCallbackCielo.CanceledCard;
                    transaction.Status = StatusPaymentEnum.ErrorPayment;
                    transaction.Details = Consts.StatusCallbackCielo.CanceledCard;
                    break;
                case StatusCallbackCielo.CreditCardProblems:
                    _messageReturn.Message = Consts.StatusCallbackCielo.CreditCardProblems;
                    transaction.Status = StatusPaymentEnum.ErrorPayment;
                    transaction.Details = Consts.StatusCallbackCielo.CreditCardProblems;
                    break;
                case StatusCallbackCielo.SuccessfullyPerformedOperation2:
                    _messageReturn.Data = Consts.StatusCallbackCielo.SuccessfullyPerformedOperation;
                    transaction.Status = StatusPaymentEnum.Aproved;
                    transaction.Details = Consts.StatusCallbackCielo.SuccessfullyPerformedOperation;
                    break;
            }
            transaction.PaymentIdService = obj.Payment.PaymentId;
        }
        private async Task PaymentCieloSlipAsync(Transaction transaction)
        {
            try
            {
                var transactionToJson = new PaymentCieloSlip()
                {
                    MerchantOrderId = "2014111703",
                    Customer = new Model.Cielo.Customer()
                    {
                        Name = "Comprador Teste Boleto",
                        Identity = "1234567890",
                        Address = new Model.Cielo.Address
                        {
                            Street = "Avenida Marechal Câmara",
                            Number = "160",
                            Complement = "Sala 934",
                            ZipCode = "22750012",
                            District = "Centro",
                            City = "Rio de Janeiro",
                            State = "RJ",
                            Country = "BRA"
                        }
                    },
                    Payment = new Model.Cielo.Payment()
                    {
                        Provider = "bradesco2",
                        //"Address" = "teste rua",
                        BoletoNumber = "123",
                        Assignor = "Empresa Teste",
                        Demonstrative = "Desmonstrative Teste",
                        ExpirationDate = "5/1/2023",
                        Identification = "11884926754",
                        Instructions = "Aceitar somente até a data de vencimento, após essa data juros de 1% dia.",
                        Amount = 500,
                        Type = "Boleto",
                    }
                };
                var transactionJson = new StringContent(System.Text.Json.JsonSerializer.Serialize(transactionToJson), Encoding.UTF8, Application.Json);
                var objJson = SendRequestAsync(transactionJson);
                var obj = JsonConvert.DeserializeObject<Model.Cielo.Callback.Slip.CallbackPaymentSlip>(objJson);
                transaction.PaymentIdService = obj.Payment.PaymentId;
                _messageReturn.Data = obj.Payment.Url;
            }
            catch (System.Exception ex)
            {
                throw;
            }

        }

        private string SendRequestAsync(StringContent transactionJson)
        {
            try
            {
                var requestMessage = new HttpRequestMessage(HttpMethod.Post, "https://apisandbox.cieloecommerce.cielo.com.br/1/sales");
                requestMessage.Headers.Add("Accept", "*/*");
                requestMessage.Headers.Add("Accept-Encoding", "gzip, deflate, br");
                requestMessage.Headers.Add("Connection", "keep-alive");
                requestMessage.Headers.Add("MerchantId", "e3169e28-d8e5-4f90-8db2-3c54af7d361a");
                requestMessage.Headers.Add("MerchantKey", "GNOWRLOKZITAJZTNKLDTZNEAUDHFQTRCTAKMWQEP");
                requestMessage.Content = transactionJson;

                using var httpResponseMessage = _HttpClient.Send(requestMessage);

                string jsonContent = httpResponseMessage.Content.ReadAsStringAsync().Result;
                return jsonContent;
            }
            catch (System.Exception ex)
            {
                throw;
            }
        }
    }
}