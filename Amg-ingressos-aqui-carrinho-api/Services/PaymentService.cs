using System.Text;
using Amg_ingressos_aqui_carrinho_api.Infra;
using Amg_ingressos_aqui_carrinho_api.Model;
using Amg_ingressos_aqui_carrinho_api.Model.Cielo.Callback;
using Amg_ingressos_aqui_carrinho_api.Model.Cielo.CreditCard;
using Amg_ingressos_aqui_carrinho_api.Model.Cielo.DebitCard;
using Amg_ingressos_aqui_carrinho_api.Model.Cielo.Pix;
using Amg_ingressos_aqui_carrinho_api.Services.Interfaces;
using Amg_ingressos_aqui_carrinho_api.Enum;
using Newtonsoft.Json;
using static System.Net.Mime.MediaTypeNames;
using Amg_ingressos_aqui_carrinho_api.Model.Cielo;
using Amg_ingressos_aqui_carrinho_api.Exceptions;
using Microsoft.Extensions.Options;

namespace Amg_ingressos_aqui_carrinho_api.Services
{
    public class PaymentService : IPaymentService
    {
        private MessageReturn _messageReturn;
        private HttpClient _HttpClient;
        private IUserService _userService;
        private ITransactionGatewayClient transactionClient;

        public PaymentService(IOptions<PaymentSettings> transactionDatabaseSettings, IUserService userService)
        {
            if(transactionDatabaseSettings.Value.Key.Equals("PAGBANK")){
                //_HttpClient = new CieloClient(transactionDatabaseSettings).CreateClient();
                transactionClient = new PagBankClient(transactionDatabaseSettings);
            }
            else
                ;//_HttpClient = new CieloClient(transactionDatabaseSettings).CreateClient();
            
            _userService = userService;
            _messageReturn = new Model.MessageReturn();
        }

        public async Task<MessageReturn> Payment(Transaction transaction)
        {
            var result = _userService.FindByIdAsync(transaction.IdPerson).Result;
            User user = result.Data as User;
            try
            {
                StringContent transactionJson;
                if (transaction.PaymentMethod.TypePayment == Enum.TypePaymentEnum.CreditCard)
                    //await PaymentCieloCreditCardAsync(transaction);
                    _messageReturn = await transactionClient.PaymentCreditCardAsync(transaction,user);
                else if (transaction.PaymentMethod.TypePayment == Enum.TypePaymentEnum.DebitCard)
                    //await PaymentCieloDebitCardAsync(transaction);
                    _messageReturn = await transactionClient.PaymentDebitCardAsync(transaction);
                else if (transaction.PaymentMethod.TypePayment == Enum.TypePaymentEnum.PaymentSlip)
                    //PaymentCieloSlipAsync(transaction);
                    _messageReturn = await transactionClient.PaymentSlipAsync(transaction);
                else
                    throw new Exception("Tipo nao mapeado");
            }
            catch (CreditCardNotValidExeption ex)
            {
                _messageReturn.Message = ex.Message;
                throw ex;
            }
            catch (System.Exception ex)
            {
                _messageReturn.Message = ex.Message;
                throw ex;
            }
            return _messageReturn;
        }

        public async Task<MessageReturn> PaymentCieloPixAsync(Transaction transaction)
        {
            try
            {
                var result = _userService.FindByIdAsync(transaction.IdPerson).Result;

                User user = result.Data as User;

                var transactionToJson = transaction.PaymentMethodPix;

                var transactionJson = new StringContent(
                    System.Text.Json.JsonSerializer.Serialize(transactionToJson),
                    Encoding.UTF8,
                    Application.Json
                );
                var objJson = SendRequestAsync(transactionJson);
                var obj = JsonConvert.DeserializeObject<CallbackPix>(objJson);

                // switch (obj.Payment.ReturnCode)
                // {
                //     case StatusCallbackCielo.Pending:
                //         _messageReturn.Data = Consts
                //             .StatusCallbackCielo
                //             .SuccessfullyPerformedOperation;
                //         transaction.Status = StatusPaymentEnum.Pending;
                //         break;
                // }

                _messageReturn.Data = obj;
            }
            catch (Exception ex)
            {
                _messageReturn.Message = ex.Message;
            }

            return _messageReturn;
        }

        private async Task<object> PaymentCieloDebitCardAsync(Transaction transaction)
        {
            var result = _userService.FindByIdAsync(transaction.IdPerson).Result;
            if (result.Message.Any())
                throw new Exception(result.Message);

            User user = result.Data as User;

            var transactionToJson = new PaymentCieloDebitCard()
            {
                MerchantOrderId = transaction.Id,
                Customer = new Model.Cielo.DebitCard.Customer() { Name = user.Name },
                Payment = new Model.Cielo.DebitCard.Payment()
                {
                    Amount = (int)(
                        (transaction.TotalValue + transaction.Tax) - transaction.Discount
                    ),
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

            var transactionJson = new StringContent(
                System.Text.Json.JsonSerializer.Serialize(transactionToJson),
                Encoding.UTF8,
                Application.Json
            );
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

            return obj;
        }

        private async Task<object> PaymentCieloCreditCardAsync(Transaction transaction)
        {
            try
            {
                var transactionToJson = new PaymentCieloCreditCard()
                {
                    MerchantOrderId = transaction.Id,
                    Payment = new Model.Cielo.CreditCard.Payment()
                    {
                        Amount = (int)(
                            (transaction.TotalValue + transaction.Tax) - transaction.Discount
                        ),
                        CreditCard = new Model.Cielo.CreditCard.CreditCard()
                        {
                            Brand = transaction.PaymentMethod.Brand,
                            CardNumber = transaction.PaymentMethod.CardNumber,
                            ExpirationDate = transaction.PaymentMethod.ExpirationDate,
                            Holder = transaction.PaymentMethod.Holder,
                            SecurityCode = transaction.PaymentMethod.SecurityCode,
                            SaveCard = "false",
                            CardOnFile = new CardOnFile()
                        },
                        Type = transaction.PaymentMethod.TypePayment.ToString(),
                        Installments = 1,
                        SoftDescriptor = "Tiquetera"
                    }
                };
                // var transactionToJson = new PaymentCieloCreditCard()
                //     {
                //         MerchantOrderId = "2014111703",
                //         Payment = new Model.Cielo.CreditCard.Payment()
                //         {
                //             Amount = 500,//(int)transaction.TransactionItens.Sum(i => i.TicketPrice),
                //             CreditCard = new Model.Cielo.CreditCard.CreditCard()
                //             {
                //                 Brand = "Visa",
                //                 CardNumber = "4551870000000184",
                //                 ExpirationDate = "12/2021",
                //                 Holder = "Teste Holder",
                //                 SecurityCode = "123"
                //             },
                //             Type = "CreditCard",
                //             Installments = 1,
                //             SoftDescriptor = "Tiquetera"
                //         }
                //     };

                var transactionJson = new StringContent(
                    System.Text.Json.JsonSerializer.Serialize(transactionToJson),
                    Encoding.UTF8,
                    Application.Json
                );
                var creditCard = new StringContent(
                    System.Text.Json.JsonSerializer.Serialize(transactionToJson.Payment.CreditCard),
                    Encoding.UTF8,
                    Application.Json
                );
                var cardIsValid = ValidateCard(creditCard).Result;

                if (!cardIsValid.Valid)
                    throw new CreditCardNotValidExeption(cardIsValid.ReturnMessage);

                var objJson = SendRequestAsync(transactionJson);
                var obj = JsonConvert.DeserializeObject<CallbackCreditCard>(objJson.ToString());
                switch (obj.Payment.ReturnCode)
                {
                    case StatusCallbackCielo.SuccessfullyPerformedOperation:
                        _messageReturn.Data = Consts
                            .StatusCallbackCielo
                            .SuccessfullyPerformedOperation;
                        transaction.Status = StatusPaymentEnum.Aproved;
                        transaction.Details = Consts
                            .StatusCallbackCielo
                            .SuccessfullyPerformedOperation;
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
                        _messageReturn.Data = Consts
                            .StatusCallbackCielo
                            .SuccessfullyPerformedOperation;
                        transaction.Status = StatusPaymentEnum.Aproved;
                        transaction.Details = Consts
                            .StatusCallbackCielo
                            .SuccessfullyPerformedOperation;
                        break;
                }
                transaction.PaymentIdService = obj.Payment.PaymentId;

                return obj;
            }
            catch (CreditCardNotValidExeption ex)
            {
                throw ex;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        private async Task<CreditCardIsValid> ValidateCard(StringContent transactionJson)
        {
            try
            {
                var requestMessage = new HttpRequestMessage(
                    HttpMethod.Post,
                    "https://apisandbox.cieloecommerce.cielo.com.br/1/zeroauth"
                );
                requestMessage.Headers.Add("Accept", "*/*");
                requestMessage.Headers.Add("Accept-Encoding", "gzip, deflate, br");
                requestMessage.Headers.Add("Connection", "keep-alive");
                requestMessage.Headers.Add("MerchantId", "e3169e28-d8e5-4f90-8db2-3c54af7d361a");
                requestMessage.Headers.Add(
                    "MerchantKey",
                    "GNOWRLOKZITAJZTNKLDTZNEAUDHFQTRCTAKMWQEP"
                );
                requestMessage.Content = transactionJson;

                using var httpResponseMessage = _HttpClient.Send(requestMessage);

                string jsonContent = httpResponseMessage.Content.ReadAsStringAsync().Result;
                var result = httpResponseMessage.EnsureSuccessStatusCode();

                return JsonConvert.DeserializeObject<CreditCardIsValid>(jsonContent);
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        private async Task PaymentCieloSlipAsync(Transaction transaction)
        {
            try
            {
                var result = _userService.FindByIdAsync(transaction.IdPerson).Result;
                if (result.Message.Any())
                    throw new Exception(result.Message);

                User user = result.Data as User;

                var transactionToJson = new PaymentCieloSlip()
                {
                    MerchantOrderId = transaction.Id,
                    Customer = new Model.Cielo.Customer()
                    {
                        Name = user.Name,
                        Identity = user.DocumentId,
                        Address = new Model.Cielo.Address
                        {
                            Street = user.Address.AddressDescription,
                            Number = user.Address.Number,
                            Complement = user.Address.Complement,
                            ZipCode = user.Address.Cep,
                            District = user.Address.Neighborhood,
                            City = user.Address.City,
                            State = user.Address.State,
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
                        Instructions =
                            "Aceitar somente até a data de vencimento, após essa data juros de 1% dia.",
                        Amount = transaction.TotalValue,
                        Type = "Boleto",
                    }
                };
                var transactionJson = new StringContent(
                    System.Text.Json.JsonSerializer.Serialize(transactionToJson),
                    Encoding.UTF8,
                    Application.Json
                );
                var objJson = SendRequestAsync(transactionJson);
                var obj =
                    JsonConvert.DeserializeObject<Model.Cielo.Callback.Slip.CallbackPaymentSlip>(
                        objJson
                    );
                transaction.PaymentIdService = obj.Payment.PaymentId;
                _messageReturn.Data = obj.Payment.Url;
            }
            catch (System.Exception ex)
            {
                throw;
            }
        }

        public string GetStatusPayment(string paymentId)
        {
            try
            {
                var requestMessage = new HttpRequestMessage(
                    HttpMethod.Get,
                    "https://apiquerysandbox.cieloecommerce.cielo.com.br/1/sales/" + paymentId
                );
                requestMessage.Headers.Add("Accept", "*/*");
                requestMessage.Headers.Add("Accept-Encoding", "gzip, deflate, br");
                requestMessage.Headers.Add("Connection", "keep-alive");
                requestMessage.Headers.Add("MerchantId", "e3169e28-d8e5-4f90-8db2-3c54af7d361a");
                requestMessage.Headers.Add(
                    "MerchantKey",
                    "GNOWRLOKZITAJZTNKLDTZNEAUDHFQTRCTAKMWQEP"
                );

                using var httpResponseMessage = _HttpClient.Send(requestMessage);

                string jsonContent = httpResponseMessage.Content.ReadAsStringAsync().Result;
                return jsonContent;
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
                var requestMessage = new HttpRequestMessage(
                    HttpMethod.Post,
                    "https://apisandbox.cieloecommerce.cielo.com.br/1/sales"
                );
                requestMessage.Headers.Add("Accept", "*/*");
                requestMessage.Headers.Add("Accept-Encoding", "gzip, deflate, br");
                requestMessage.Headers.Add("Connection", "keep-alive");
                requestMessage.Headers.Add("MerchantId", "e3169e28-d8e5-4f90-8db2-3c54af7d361a");
                requestMessage.Headers.Add(
                    "MerchantKey",
                    "GNOWRLOKZITAJZTNKLDTZNEAUDHFQTRCTAKMWQEP"
                );
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
