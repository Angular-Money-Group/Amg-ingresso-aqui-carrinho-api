using System.Text;
using Amg_ingressos_aqui_carrinho_api.Model;
using Amg_ingressos_aqui_carrinho_api.Enum;
using Newtonsoft.Json;
using Microsoft.Extensions.Options;
using Amg_ingressos_aqui_carrinho_api.Dto.Pagbank;
using Amg_ingressos_aqui_carrinho_api.Model.Pagbank;
using Amg_ingressos_aqui_carrinho_api.Model.Pagbank.Callback;
using Amg_ingressos_aqui_carrinho_api.Dto;
using Amg_ingressos_aqui_carrinho_api.Model.Pagbank.Pix;
using Amg_ingressos_aqui_carrinho_api.Model.Pagbank.Callback.Boleto;
using Amg_ingressos_aqui_carrinho_api.Consts;

namespace Amg_ingressos_aqui_carrinho_api.Infra
{
    public class PagBankClient : ITransactionGatewayClient
    {
        private readonly IOptions<PaymentSettings> _config;
        private readonly MessageReturn _messageReturn;
        private readonly HttpClient _httpClient = new HttpClient();
        private readonly string _url;
        private readonly ILogger<PagBankClient> _logger;

        public PagBankClient(IOptions<PaymentSettings> transactionDatabaseSettings)
        {
            _config = transactionDatabaseSettings;
            _messageReturn = new MessageReturn();
            _url = _config.Value.PagBankSettings.UrlApiHomolog + "/orders";
        }
        public PagBankClient(IOptions<PaymentSettings> transactionDatabaseSettings, ILogger<PagBankClient> logger)
        {
            _config = transactionDatabaseSettings;
            _messageReturn = new MessageReturn();
            _url = _config.Value.PagBankSettings.UrlApiHomolog + "/orders";
            _logger = logger;
        }

        public async Task<MessageReturn> PaymentSlipAsync(Transaction transaction, User user)
        {

            try
            {
                //cria pedido e paga
                Request request = new RequestPagBankBoletoDto().TransactionToRequest(transaction, user);
                Response response = new OperatorRest().SendRequestAsync(request, _url, _config.Value.PagBankSettings.TokenHomolog);

                if (string.IsNullOrEmpty(response.Data))
                {
                    _messageReturn.Message = response.Message;
                    transaction.Status = StatusPayment.ErrorPayment;
                    transaction.Details = response.Message;
                }

                var obj = JsonConvert.DeserializeObject<CallbackBoletoPagBank>(response.Data);
                transaction.PaymentIdService = obj.Id;
                _messageReturn.Data = "ok";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, string.Format(MessageLogErrors.Process, this.GetType().Name, nameof(PaymentSlipAsync)));
                throw;
            }

            return _messageReturn;
        }

        public async Task<MessageReturn> PaymentCreditCardAsync(Transaction transaction, User user)
        {
            try
            {
                //valida cartao
                //var cardIsValid = ValidateCard(transaction).Result;

                //if (!cardIsValid)
                //throw new CreditCardNotValidExeption("cartao expirado");

                //cria pedido e paga
                Request request = new RequestPagBankCardDto().TransactionToRequest(transaction, user);
                Response response = new OperatorRest().SendRequestAsync(request, _url, _config.Value.PagBankSettings.TokenHomolog);

                if (string.IsNullOrEmpty(response.Data))
                {
                    _messageReturn.Message = response.Message;
                    transaction.Status = StatusPayment.ErrorPayment;
                    transaction.Details = response.Message;
                }

                var obj = JsonConvert.DeserializeObject<CallbackCreditCardPagBank>(response.Data);
                if (obj.Charges.FirstOrDefault().Status.ToUpper() == "DECLINED")
                    _messageReturn.Message = obj.Charges.FirstOrDefault().PaymentResponse.Message;
                else
                {
                    transaction.PaymentIdService = obj.Id;
                    _messageReturn.Data = "ok";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, string.Format(MessageLogErrors.Process, this.GetType().Name, nameof(PaymentCreditCardAsync)));
                throw;
            }

            return _messageReturn;
        }

        public async Task<MessageReturn> PaymentDebitCardAsync(Transaction transaction, User user)
        {
            try
            {
                //valida cartao
                //var cardIsValid = ValidateCard(transaction).Result;

                //if (!cardIsValid)
                //throw new CreditCardNotValidExeption("cartao expirado");

                //cria pedido e paga
                Request request = new RequestPagBankCardDto().TransactionToRequest(transaction, user);
                Response response = new OperatorRest().SendRequestAsync(request, _url, _config.Value.PagBankSettings.TokenHomolog);

                if (string.IsNullOrEmpty(response.Data))
                {
                    _messageReturn.Message = response.Message;
                    transaction.Status = StatusPayment.ErrorPayment;
                    transaction.Details = response.Message;
                }

                var obj = JsonConvert.DeserializeObject<CallbackCreditCardPagBank>(response.Data);
                if (obj.Charges.FirstOrDefault().Status.ToUpper() == "DECLINED")
                    _messageReturn.Message = obj.Charges.FirstOrDefault().PaymentResponse.Message;
                else
                {
                    transaction.PaymentIdService = obj.Id;
                    _messageReturn.Data = "Ok";
                }

                return _messageReturn;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, string.Format(MessageLogErrors.Process, this.GetType().Name, nameof(PaymentDebitCardAsync)));
                throw;
            }
        }

        public async Task<MessageReturn> PaymentPixAsync(Transaction transaction, User user)
        {
            try
            {
                //cria pedido e paga
                Request request = new RequestPagBankPixDto().TransactionToRequest(transaction, user);
                Response response = new OperatorRest().SendRequestAsync(request, _url, _config.Value.PagBankSettings.TokenHomolog);

                if (!string.IsNullOrEmpty(response.Data))
                {
                    StringBuilder messagejson = new StringBuilder();
                    JsonConvert.DeserializeObject<CallbackErrorMessagePagBank>(response.Message).ErrorMessages.ForEach(x =>
                    {
                        messagejson.Append(x.Code + " = " + x.ParameterName + " -- Error:" + x.Description + " : " + x.ParameterName);
                    });
                    response.Message = messagejson.ToString();
                    _messageReturn.Message = response.Message;
                    transaction.Status = StatusPayment.ErrorPayment;
                    transaction.Details = response.Message;
                }

                var obj = JsonConvert.DeserializeObject<CallbackPixPagBank>(response.Data);
                transaction.PaymentIdService = obj.id;
                _messageReturn.Data = obj.QrCodes.FirstOrDefault()
                                         .links.FirstOrDefault(i => i.Rel.Equals("QRCODE.PNG"))
                                         .Href;
                return _messageReturn;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, string.Format(MessageLogErrors.Process, this.GetType().Name, nameof(PaymentDebitCardAsync)));
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