using System.Text;
using Amg_ingressos_aqui_carrinho_api.Consts;
using Amg_ingressos_aqui_carrinho_api.Enum;
using Amg_ingressos_aqui_carrinho_api.Exceptions;
using Amg_ingressos_aqui_carrinho_api.Model;
using Amg_ingressos_aqui_carrinho_api.Model.Cielo;
using Amg_ingressos_aqui_carrinho_api.Model.Cielo.Callback;
using Amg_ingressos_aqui_carrinho_api.Model.Cielo.Callback.Slip;
using Amg_ingressos_aqui_carrinho_api.Model.Cielo.CreditCard;
using Amg_ingressos_aqui_carrinho_api.Model.Cielo.DebitCard;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using static System.Net.Mime.MediaTypeNames;

namespace Amg_ingressos_aqui_carrinho_api.Infra
{
    public class CieloClient : ITransactionGatewayClient
    {
        private readonly HttpClient _httpClient;
        private IOptions<PaymentSettings> _config;
        private MessageReturn _messageReturn;
        private string _url;
        private readonly ILogger<CieloClient> _logger;

        public CieloClient(IOptions<PaymentSettings> transactionDatabaseSettings, ILogger<CieloClient> logger)
        {
            _config = transactionDatabaseSettings;
            _messageReturn = new MessageReturn();
            _url = _config.Value.PagBankSettings.UrlApiHomolog + "/orders";
            _logger = logger;
        }
        public CieloClient(IOptions<PaymentSettings> transactionDatabaseSettings)
        {
            _config = transactionDatabaseSettings;
            _messageReturn = new MessageReturn();
            _url = _config.Value.PagBankSettings.UrlApiHomolog + "/orders";
        }

        public HttpClient CreateClient()
        {
            _httpClient.BaseAddress = new Uri(_config.Value.CieloSettings.UrlApiHomolog);
            _httpClient.DefaultRequestHeaders.Add(
                HeaderNames.Accept, "application/vnd.github.v3+json");
            _httpClient.DefaultRequestHeaders.Add(
                HeaderNames.UserAgent, "HttpRequestsSample");
            _httpClient.DefaultRequestHeaders.Add(
                "MerchantId", _config.Value.CieloSettings.MerchantIdHomolog);
            _httpClient.DefaultRequestHeaders.Add(
                "MerchantKey", _config.Value.CieloSettings.MerchantKeyHomolog);
            _httpClient.Timeout = TimeSpan.FromMinutes(10);

            return _httpClient;
        }

        public async Task<MessageReturn> PaymentCreditCardAsync(Transaction transaction, User user)
        {
            try
            {
                var transactionToJson = new PaymentCieloCreditCard()
                {
                    MerchantOrderId = transaction.Id,
                    Payment = new Model.Cielo.Payment()
                    {
                        Amount = (int)(transaction.TotalValue + transaction.Tax - transaction.Discount),
                        CreditCard = new CreditCardCielo()
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
                var cardIsValid = ValidateCard(transactionToJson.Payment.CreditCard).Result;

                if (!cardIsValid.Valid)
                    throw new CreditCardNotValidExeption(cardIsValid.ReturnMessage);

                Request request = new Request() { Data = System.Text.Json.JsonSerializer.Serialize(transactionToJson) };
                Response response = new OperatorRest().SendRequestAsync(request, _url, _config.Value.CieloSettings.MerchantIdHomolog);

                if (string.IsNullOrEmpty(response.Data))
                {
                    _messageReturn.Message = response.Message;
                    transaction.Status = StatusPaymentEnum.ErrorPayment;
                    transaction.Details = response.Message;
                }

                var obj = JsonConvert.DeserializeObject<CallbackCreditCard>(response.Data);
                transaction.PaymentIdService = obj.Payment.PaymentId;
                
                switch (obj.Payment.ReturnCode)
                {
                    case StatusCallbackCielo.SuccessfullyPerformedOperation:
                        _messageReturn.Data = Consts
                            .TypeStatusCallbackCielo
                            .SuccessfullyPerformedOperation;
                        transaction.Status = StatusPaymentEnum.Aproved;
                        transaction.Details = Consts
                            .TypeStatusCallbackCielo
                            .SuccessfullyPerformedOperation;
                        break;
                    case StatusCallbackCielo.NotAllowed:
                        _messageReturn.Message = Consts.TypeStatusCallbackCielo.NotAllowed;
                        transaction.Status = StatusPaymentEnum.ErrorPayment;
                        transaction.Details = Consts.TypeStatusCallbackCielo.NotAllowed;
                        break;
                    case StatusCallbackCielo.ExpiredCard:
                        _messageReturn.Message = Consts.TypeStatusCallbackCielo.ExpiredCard;
                        transaction.Status = StatusPaymentEnum.ErrorPayment;
                        transaction.Details = Consts.TypeStatusCallbackCielo.ExpiredCard;
                        break;
                    case StatusCallbackCielo.BlockedCard:
                        _messageReturn.Message = Consts.TypeStatusCallbackCielo.BlockedCard;
                        transaction.Status = StatusPaymentEnum.ErrorPayment;
                        transaction.Details = Consts.TypeStatusCallbackCielo.BlockedCard;
                        break;
                    case StatusCallbackCielo.TimeOut:
                        _messageReturn.Message = Consts.TypeStatusCallbackCielo.TimeOut;
                        transaction.Status = StatusPaymentEnum.ErrorPayment;
                        transaction.Details = Consts.TypeStatusCallbackCielo.TimeOut;
                        break;
                    case StatusCallbackCielo.CanceledCard:
                        _messageReturn.Message = Consts.TypeStatusCallbackCielo.CanceledCard;
                        transaction.Status = StatusPaymentEnum.ErrorPayment;
                        transaction.Details = Consts.TypeStatusCallbackCielo.CanceledCard;
                        break;
                    case StatusCallbackCielo.CreditCardProblems:
                        _messageReturn.Message = Consts.TypeStatusCallbackCielo.CreditCardProblems;
                        transaction.Status = StatusPaymentEnum.ErrorPayment;
                        transaction.Details = Consts.TypeStatusCallbackCielo.CreditCardProblems;
                        break;
                    case StatusCallbackCielo.SuccessfullyPerformedOperation2:
                        _messageReturn.Data = Consts
                            .TypeStatusCallbackCielo
                            .SuccessfullyPerformedOperation;
                        transaction.Status = StatusPaymentEnum.Aproved;
                        transaction.Details = Consts
                            .TypeStatusCallbackCielo
                            .SuccessfullyPerformedOperation;
                        break;
                }
                transaction.PaymentIdService = obj.Payment.PaymentId;
                _messageReturn.Data = obj;
                return _messageReturn;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, string.Format(MessageLogErrors.Process, this.GetType().Name, nameof(PaymentCreditCardAsync)));
                throw;
            }
        }

        public async Task<MessageReturn> PaymentDebitCardAsync(Transaction transaction, User user)
        {
            try
            {
                var transactionToJson = new PaymentCieloDebitCard()
                {
                    MerchantOrderId = transaction.Id,
                    Customer = new Model.Cielo.Customer() { Name = user.Name },
                    Payment = new Model.Cielo.Payment()
                    {
                        Amount = (int)(transaction.TotalValue + transaction.Tax - transaction.Discount),
                        DebitCard = new DebitCardCielo()
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

                Request request = new Request() { Data = System.Text.Json.JsonSerializer.Serialize(transactionToJson) };
                Response response = new OperatorRest().SendRequestAsync(request, _url, _config.Value.CieloSettings.MerchantIdHomolog);

                if (string.IsNullOrEmpty(response.Data))
                {
                    _messageReturn.Message = response.Message;
                    transaction.Status = StatusPaymentEnum.ErrorPayment;
                    transaction.Details = response.Message;
                }

                var obj = JsonConvert.DeserializeObject<CallbackCreditCard>(response.Data);
                transaction.PaymentIdService = obj.Payment.PaymentId;

                switch (obj.Payment.ReturnCode)
                {
                    case StatusCallbackCielo.SuccessfullyPerformedOperation:
                        _messageReturn.Data = Consts.TypeStatusCallbackCielo.SuccessfullyPerformedOperation;
                        transaction.Status = StatusPaymentEnum.Aproved;
                        break;
                    case StatusCallbackCielo.NotAllowed:
                        _messageReturn.Message = Consts.TypeStatusCallbackCielo.NotAllowed;
                        transaction.Status = StatusPaymentEnum.ErrorPayment;
                        transaction.Details = Consts.TypeStatusCallbackCielo.NotAllowed;
                        break;
                    case StatusCallbackCielo.ExpiredCard:
                        _messageReturn.Message = Consts.TypeStatusCallbackCielo.ExpiredCard;
                        transaction.Status = StatusPaymentEnum.ErrorPayment;
                        transaction.Details = Consts.TypeStatusCallbackCielo.ExpiredCard;
                        break;
                    case StatusCallbackCielo.BlockedCard:
                        _messageReturn.Message = Consts.TypeStatusCallbackCielo.BlockedCard;
                        transaction.Status = StatusPaymentEnum.ErrorPayment;
                        transaction.Details = Consts.TypeStatusCallbackCielo.BlockedCard;
                        break;
                    case StatusCallbackCielo.TimeOut:
                        _messageReturn.Message = Consts.TypeStatusCallbackCielo.TimeOut;
                        transaction.Status = StatusPaymentEnum.ErrorPayment;
                        transaction.Details = Consts.TypeStatusCallbackCielo.TimeOut;
                        break;
                    case StatusCallbackCielo.CanceledCard:
                        _messageReturn.Message = Consts.TypeStatusCallbackCielo.CanceledCard;
                        transaction.Status = StatusPaymentEnum.ErrorPayment;
                        transaction.Details = Consts.TypeStatusCallbackCielo.CanceledCard;
                        break;
                    case StatusCallbackCielo.CreditCardProblems:
                        _messageReturn.Message = Consts.TypeStatusCallbackCielo.CreditCardProblems;
                        transaction.Status = StatusPaymentEnum.ErrorPayment;
                        transaction.Details = Consts.TypeStatusCallbackCielo.CreditCardProblems;
                        break;
                    case StatusCallbackCielo.SuccessfullyPerformedOperation2:
                        _messageReturn.Data = Consts.TypeStatusCallbackCielo.SuccessfullyPerformedOperation;
                        transaction.Status = StatusPaymentEnum.Aproved;
                        transaction.Details = Consts.TypeStatusCallbackCielo.SuccessfullyPerformedOperation;
                        break;
                }
                transaction.PaymentIdService = obj.Payment.PaymentId;

                _messageReturn.Data = obj;
                return _messageReturn;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, string.Format(MessageLogErrors.Process, this.GetType().Name, nameof(ValidateCard)));
                throw;
            }
        }

        public async Task<MessageReturn> PaymentPixAsync(Transaction transaction, User user)
        {
            try
            {

                var transactionToJson = transaction.PaymentMethodPix;

                Request request = new Request() { Data = System.Text.Json.JsonSerializer.Serialize(transactionToJson) };
                Response response = new OperatorRest().SendRequestAsync(request, _url, _config.Value.CieloSettings.MerchantIdHomolog);

                if (!string.IsNullOrEmpty(response.Data))
                {
                    var obj = JsonConvert.DeserializeObject<CallbackPix>(response.Data);
                    transaction.PaymentIdService = obj.Payment.PaymentId;
                    _messageReturn.Data = obj;
                }
                else
                {
                    _messageReturn.Message = response.Message;
                    transaction.Status = StatusPaymentEnum.ErrorPayment;
                    transaction.Details = response.Message;
                }

                return _messageReturn;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, string.Format(MessageLogErrors.Process, this.GetType().Name, nameof(PaymentPixAsync)));
                throw;
            }
        }

        public async Task<MessageReturn> PaymentSlipAsync(Transaction transaction, User user)
        {
            try
            {
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

                Request request = new Request() { Data = System.Text.Json.JsonSerializer.Serialize(transactionToJson) };
                Response response = new OperatorRest().SendRequestAsync(request, _url, _config.Value.PagBankSettings.TokenHomolog);

                if (!string.IsNullOrEmpty(response.Data))
                {
                    var obj = JsonConvert.DeserializeObject<CallbackPaymentSlip>(response.Data);
                    transaction.PaymentIdService = obj.Payment.PaymentId;
                    _messageReturn.Data = obj.Payment.Url;
                }
                else
                {
                    _messageReturn.Message = response.Message;
                    transaction.Status = StatusPaymentEnum.ErrorPayment;
                    transaction.Details = response.Message;
                }

                return _messageReturn;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, string.Format(MessageLogErrors.Process, this.GetType().Name, nameof(PaymentSlipAsync)));
                throw;
            }
        }

        private async Task<CreditCardIsValid> ValidateCard(CreditCardCielo transactionJson)
        {
            try
            {
                var url ="https://apisandbox.cieloecommerce.cielo.com.br/1/zeroauth";
                Request request = new Request() { Data = System.Text.Json.JsonSerializer.Serialize(transactionJson) };
                Response response = new OperatorRest().SendRequestAsync(request, url, _config.Value.CieloSettings.MerchantIdHomolog);

                if (string.IsNullOrEmpty(response.Data))
                    throw new RuleException(response.Message);

                return JsonConvert.DeserializeObject<CreditCardIsValid>(response.Data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, string.Format(MessageLogErrors.Process, this.GetType().Name, nameof(ValidateCard)));
                throw;
            }
        }

        public Task<MessageReturn> GetStatusPayment(string paymentId)
        {
            try
            {
                Request request = new Request() { Data = paymentId };
                var url = "https://apiquerysandbox.cieloecommerce.cielo.com.br/1/sales/" + paymentId;
                Response response = new OperatorRest().SendRequestAsync(request, url, _config.Value.PagBankSettings.TokenHomolog);

                if (string.IsNullOrEmpty(response.Data))
                    _messageReturn.Message = response.Message;
                _messageReturn.Data = response.Data;

                return Task.FromResult(_messageReturn);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, string.Format(MessageLogErrors.Process, this.GetType().Name, nameof(PaymentDebitCardAsync)));
                throw;
            }
        }
    }
}