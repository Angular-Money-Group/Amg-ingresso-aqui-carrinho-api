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
using Newtonsoft.Json;

namespace Amg_ingressos_aqui_carrinho_api.Infra
{
    public class CieloClient : ITransactionGatewayClient
    {
        private readonly IOptions<PaymentSettings> _config;
        private readonly MessageReturn _messageReturn;
        private readonly string _url;
        private readonly ILogger<CieloClient> _logger;
        private readonly IOperatorRest _operatorRest;


        public CieloClient(
            IOptions<PaymentSettings> transactionDatabaseSettings, 
            ILogger<CieloClient> logger,
            IOperatorRest operatorRest)
        {
            _config = transactionDatabaseSettings;
            _messageReturn = new MessageReturn();
            _url = _config.Value.PagBankSettings.UrlApiHomolog + "/orders";
            _logger = logger;
            _operatorRest = operatorRest;
        }

        public Task<MessageReturn> PaymentCreditCard(Transaction transaction, User user)
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
                            Brand = transaction.PaymentMethod.Brand ?? string.Empty,
                            CardNumber = transaction.PaymentMethod.CardNumber ?? string.Empty,
                            ExpirationDate = transaction.PaymentMethod.ExpirationDate ?? string.Empty,
                            Holder = transaction.PaymentMethod.Holder ?? string.Empty,
                            SecurityCode = transaction.PaymentMethod.SecurityCode ?? string.Empty,
                            SaveCard = "false",
                            CardOnFile = new CardOnFile()
                        },
                        Type = transaction.PaymentMethod.TypePayment.ToString(),
                        Installments = 1,
                        SoftDescriptor = "Tiquetera"
                    }
                };

                var cardIsValid = ValidateCard(transactionToJson.Payment.CreditCard);

                if (!cardIsValid.Valid)
                    throw new CreditCardNotValidException(cardIsValid.ReturnMessage);

                Request request = new Request() { Data = System.Text.Json.JsonSerializer.Serialize(transactionToJson) };
                Response response = _operatorRest.SendRequestAsync(request, _url, _config.Value.CieloSettings.MerchantIdHomolog);

                if (string.IsNullOrEmpty(response.Data))
                {
                    _messageReturn.Message = response.Message;
                    transaction.Status = StatusPayment.ErrorPayment;
                    transaction.Details = response.Message;
                }

                var obj = JsonConvert.DeserializeObject<CallbackCreditCard>(response.Data) ?? new CallbackCreditCard();
                transaction.PaymentIdService = obj.Payment.PaymentId;

                switch (obj.Payment.ReturnCode)
                {
                    case StatusCallbackCielo.SuccessfullyPerformedOperation:
                        _messageReturn.Data = TypeStatusCallbackCielo.SuccessfullyPerformedOperation;
                        transaction.Status = StatusPayment.Aproved;
                        transaction.Details = TypeStatusCallbackCielo.SuccessfullyPerformedOperation;
                        break;
                    case StatusCallbackCielo.NotAllowed:
                        _messageReturn.Message = TypeStatusCallbackCielo.NotAllowed;
                        transaction.Status = StatusPayment.ErrorPayment;
                        transaction.Details = TypeStatusCallbackCielo.NotAllowed;
                        break;
                    case StatusCallbackCielo.ExpiredCard:
                        _messageReturn.Message = TypeStatusCallbackCielo.ExpiredCard;
                        transaction.Status = StatusPayment.ErrorPayment;
                        transaction.Details = TypeStatusCallbackCielo.ExpiredCard;
                        break;
                    case StatusCallbackCielo.BlockedCard:
                        _messageReturn.Message = TypeStatusCallbackCielo.BlockedCard;
                        transaction.Status = StatusPayment.ErrorPayment;
                        transaction.Details = TypeStatusCallbackCielo.BlockedCard;
                        break;
                    case StatusCallbackCielo.TimeOut:
                        _messageReturn.Message = TypeStatusCallbackCielo.TimeOut;
                        transaction.Status = StatusPayment.ErrorPayment;
                        transaction.Details = TypeStatusCallbackCielo.TimeOut;
                        break;
                    case StatusCallbackCielo.CanceledCard:
                        _messageReturn.Message = TypeStatusCallbackCielo.CanceledCard;
                        transaction.Status = StatusPayment.ErrorPayment;
                        transaction.Details = TypeStatusCallbackCielo.CanceledCard;
                        break;
                    case StatusCallbackCielo.CreditCardProblems:
                        _messageReturn.Message = TypeStatusCallbackCielo.CreditCardProblems;
                        transaction.Status = StatusPayment.ErrorPayment;
                        transaction.Details = TypeStatusCallbackCielo.CreditCardProblems;
                        break;
                }
                transaction.PaymentIdService = obj.Payment.PaymentId;
                _messageReturn.Data = obj;
                return Task.FromResult(_messageReturn);
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format(MessageLogErrors.Process, this.GetType().Name, nameof(PaymentCreditCard)), ex);
                throw;
            }
        }

        public Task<MessageReturn> PaymentDebitCard(Transaction transaction, User user)
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
                            Brand = transaction.PaymentMethod.Brand ?? string.Empty,
                            CardNumber = transaction.PaymentMethod.CardNumber ?? string.Empty,
                            ExpirationDate = transaction.PaymentMethod.ExpirationDate ?? string.Empty,
                            Holder = transaction.PaymentMethod.Holder ?? string.Empty,
                            SecurityCode = transaction.PaymentMethod.SecurityCode ?? string.Empty
                        },
                        Type = transaction.PaymentMethod.TypePayment.ToString(),
                        Provider = "Simulado",
                        ReturnUrl = "retorno url"
                    }
                };

                Request request = new Request() { Data = System.Text.Json.JsonSerializer.Serialize(transactionToJson) };
                Response response = _operatorRest.SendRequestAsync(request, _url, _config.Value.CieloSettings.MerchantIdHomolog);

                if (string.IsNullOrEmpty(response.Data))
                {
                    _messageReturn.Message = response.Message;
                    transaction.Status = StatusPayment.ErrorPayment;
                    transaction.Details = response.Message;
                }

                var obj = JsonConvert.DeserializeObject<CallbackCreditCard>(response.Data) ?? new CallbackCreditCard();
                transaction.PaymentIdService = obj.Payment.PaymentId;

                switch (obj.Payment.ReturnCode)
                {
                    case StatusCallbackCielo.SuccessfullyPerformedOperation:
                        _messageReturn.Data = TypeStatusCallbackCielo.SuccessfullyPerformedOperation;
                        transaction.Status = StatusPayment.Aproved;
                        break;
                    case StatusCallbackCielo.NotAllowed:
                        _messageReturn.Message = TypeStatusCallbackCielo.NotAllowed;
                        transaction.Status = StatusPayment.ErrorPayment;
                        transaction.Details = TypeStatusCallbackCielo.NotAllowed;
                        break;
                    case StatusCallbackCielo.ExpiredCard:
                        _messageReturn.Message = TypeStatusCallbackCielo.ExpiredCard;
                        transaction.Status = StatusPayment.ErrorPayment;
                        transaction.Details = TypeStatusCallbackCielo.ExpiredCard;
                        break;
                    case StatusCallbackCielo.BlockedCard:
                        _messageReturn.Message = TypeStatusCallbackCielo.BlockedCard;
                        transaction.Status = StatusPayment.ErrorPayment;
                        transaction.Details = TypeStatusCallbackCielo.BlockedCard;
                        break;
                    case StatusCallbackCielo.TimeOut:
                        _messageReturn.Message = TypeStatusCallbackCielo.TimeOut;
                        transaction.Status = StatusPayment.ErrorPayment;
                        transaction.Details = TypeStatusCallbackCielo.TimeOut;
                        break;
                    case StatusCallbackCielo.CanceledCard:
                        _messageReturn.Message = TypeStatusCallbackCielo.CanceledCard;
                        transaction.Status = StatusPayment.ErrorPayment;
                        transaction.Details = TypeStatusCallbackCielo.CanceledCard;
                        break;
                    case StatusCallbackCielo.CreditCardProblems:
                        _messageReturn.Message = TypeStatusCallbackCielo.CreditCardProblems;
                        transaction.Status = StatusPayment.ErrorPayment;
                        transaction.Details = TypeStatusCallbackCielo.CreditCardProblems;
                        break;
                    case StatusCallbackCielo.SuccessfullyPerformedOperation2:
                        _messageReturn.Data = TypeStatusCallbackCielo.SuccessfullyPerformedOperation;
                        transaction.Status = StatusPayment.Aproved;
                        transaction.Details = TypeStatusCallbackCielo.SuccessfullyPerformedOperation;
                        break;
                }
                transaction.PaymentIdService = obj.Payment.PaymentId;

                _messageReturn.Data = obj;
                return Task.FromResult(_messageReturn);
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format(MessageLogErrors.Process, this.GetType().Name, nameof(ValidateCard)), ex);
                throw;
            }
        }

        public Task<MessageReturn> PaymentPix(Transaction transaction, User user)
        {
            try
            {

                var transactionToJson ="";

                Request request = new Request() { Data = System.Text.Json.JsonSerializer.Serialize(transactionToJson) };
                Response response = _operatorRest.SendRequestAsync(request, _url, _config.Value.CieloSettings.MerchantIdHomolog);

                if (!string.IsNullOrEmpty(response.Data))
                {
                    var obj = JsonConvert.DeserializeObject<CallbackPix>(response.Data) ?? new CallbackPix();
                    transaction.PaymentIdService = obj.Payment.PaymentId;
                    _messageReturn.Data = obj;
                }
                else
                {
                    _messageReturn.Message = response.Message;
                    transaction.Status = StatusPayment.ErrorPayment;
                    transaction.Details = response.Message;
                }

                return Task.FromResult(_messageReturn);
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format(MessageLogErrors.Process, this.GetType().Name, nameof(PaymentPix)), ex);
                throw;
            }
        }

        public Task<MessageReturn> PaymentSlip(Transaction transaction, User user)
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
                            Street = user.Address.AddressDescription ?? string.Empty,
                            Number = user.Address.Number ?? string.Empty,
                            Complement = user.Address.Complement ?? string.Empty,
                            ZipCode = user.Address.Cep ?? string.Empty,
                            District = user.Address.Neighborhood ?? string.Empty,
                            City = user.Address.City ?? string.Empty,
                            State = user.Address.State ?? string.Empty,
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
                Response response = _operatorRest.SendRequestAsync(request, _url, _config.Value.PagBankSettings.TokenHomolog);

                if (!string.IsNullOrEmpty(response.Data))
                {
                    var obj = JsonConvert.DeserializeObject<CallbackPaymentSlip>(response.Data) ?? new CallbackPaymentSlip();
                    transaction.PaymentIdService = obj.Payment.PaymentId;
                    _messageReturn.Data = obj.Payment.Url;
                }
                else
                {
                    _messageReturn.Message = response.Message;
                    transaction.Status = StatusPayment.ErrorPayment;
                    transaction.Details = response.Message;
                }

                return Task.FromResult(_messageReturn);
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format(MessageLogErrors.Process, this.GetType().Name, nameof(PaymentSlip)), ex);
                throw;
            }
        }

        private CreditCardIsValid ValidateCard(CreditCardCielo transactionJson)
        {
            try
            {
                var url = Settings.CieloZeroAuth;
                Request request = new Request() { Data = System.Text.Json.JsonSerializer.Serialize(transactionJson) };
                Response response = _operatorRest.SendRequestAsync(request, url, _config.Value.CieloSettings.MerchantIdHomolog);

                if (string.IsNullOrEmpty(response.Data))
                    throw new RuleException(response.Message);

                return JsonConvert.DeserializeObject<CreditCardIsValid>(response.Data) ?? new CreditCardIsValid();
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format(MessageLogErrors.Process, this.GetType().Name, nameof(ValidateCard)), ex);
                throw;
            }
        }

        public Task<MessageReturn> GetStatusPayment(string paymentId)
        {
            try
            {
                Request request = new Request() { Data = paymentId };
                var url = Settings.CieloStatusPayment + paymentId;
                Response response = _operatorRest.SendRequestAsync(request, url, _config.Value.PagBankSettings.TokenHomolog);

                if (string.IsNullOrEmpty(response.Data))
                    _messageReturn.Message = response.Message;
                _messageReturn.Data = response.Data;

                return Task.FromResult(_messageReturn);
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format(MessageLogErrors.Process, this.GetType().Name, nameof(PaymentDebitCard)), ex);
                throw;
            }
        }
    }
}