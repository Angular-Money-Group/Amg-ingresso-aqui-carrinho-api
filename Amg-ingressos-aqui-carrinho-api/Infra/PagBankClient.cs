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
        private readonly string _url;
        private readonly ILogger<PagBankClient> _logger;

        public PagBankClient(IOptions<PaymentSettings> transactionDatabaseSettings, ILogger<PagBankClient> logger)
        {
            _config = transactionDatabaseSettings;
            _messageReturn = new MessageReturn();
            _url = _config.Value.PagBankSettings.UrlApiHomolog + "/orders";
            _logger = logger;
        }

        public Task<MessageReturn> PaymentSlip(Transaction transaction, User user)
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

                var obj = JsonConvert.DeserializeObject<CallbackBoletoPagBank>(response.Data) ?? new CallbackBoletoPagBank();
                transaction.PaymentIdService = obj.Id;
                _messageReturn.Data = "ok";
                return Task.FromResult(_messageReturn);

            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format(MessageLogErrors.Process, this.GetType().Name, nameof(PaymentSlip)), ex);
                throw;
            }
        }

        public Task<MessageReturn> PaymentCreditCard(Transaction transaction, User user)
        {
            try
            {
                //valida cartao

                //cria pedido e paga
                Request request = new RequestPagBankCardDto().TransactionToRequest(transaction, user);
                Response response = new OperatorRest().SendRequestAsync(request, _url, _config.Value.PagBankSettings.TokenHomolog);

                if (string.IsNullOrEmpty(response.Data))
                {
                    _messageReturn.Message = response.Message;
                    transaction.Status = StatusPayment.ErrorPayment;
                    transaction.Details = response.Message;
                }
                else
                {
                    var obj = JsonConvert.DeserializeObject<CallbackCreditCardPagBank>(response.Data) ?? new CallbackCreditCardPagBank();
                    if (obj.Charges[0].Status.ToUpper() == "DECLINED")
                        _messageReturn.Message = obj.Charges[0].PaymentResponse.Message;
                    else
                    {
                        transaction.PaymentIdService = obj.Id;
                        _messageReturn.Data = "ok";
                    }
                }

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
                //valida cartao

                //cria pedido e paga
                Request request = new RequestPagBankCardDto().TransactionToRequest(transaction, user);
                Response response = new OperatorRest().SendRequestAsync(request, _url, _config.Value.PagBankSettings.TokenHomolog);

                if (string.IsNullOrEmpty(response.Data))
                {
                    _messageReturn.Message = response.Message;
                    transaction.Status = StatusPayment.ErrorPayment;
                    transaction.Details = response.Message;
                }
                else
                {
                    var obj = JsonConvert.DeserializeObject<CallbackCreditCardPagBank>(response.Data) ?? new CallbackCreditCardPagBank();
                    if (obj.Charges[0].Status.ToUpper() == "DECLINED")
                        _messageReturn.Message = obj.Charges[0].PaymentResponse.Message;
                    else
                    {
                        transaction.PaymentIdService = obj.Id;
                        _messageReturn.Data = "Ok";
                    }
                }

                return Task.FromResult(_messageReturn);
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format(MessageLogErrors.Process, this.GetType().Name, nameof(PaymentDebitCard)), ex);
                throw;
            }
        }

        public Task<MessageReturn> PaymentPix(Transaction transaction, User user)
        {
            try
            {
                //cria pedido e paga
                Request request = new RequestPagBankPixDto().TransactionToRequest(transaction, user);
                Response response = new OperatorRest().SendRequestAsync(request, _url, _config.Value.PagBankSettings.TokenHomolog);

                if (!string.IsNullOrEmpty(response.Data))
                {
                    StringBuilder messagejson = new StringBuilder();
                    var messageError = JsonConvert.DeserializeObject<CallbackErrorMessagePagBank>(response.Message) ?? new CallbackErrorMessagePagBank();

                    messageError.ErrorMessages.ForEach(x =>
                    {
                        messagejson.Append(x.Code + " = " + x.ParameterName + " -- Error:" + x.Description + " : " + x.ParameterName);
                    });
                    response.Message = messagejson.ToString();
                    _messageReturn.Message = response.Message;
                    transaction.Status = StatusPayment.ErrorPayment;
                    transaction.Details = response.Message;
                }

                var obj = JsonConvert.DeserializeObject<CallbackPixPagBank>(response.Data) ?? new CallbackPixPagBank();
                transaction.PaymentIdService = obj.id;
                _messageReturn.Data = obj.QrCodes[0]
                                         .links.First(i => i.Rel.Equals("QRCODE.PNG"))
                                         .Href;
                return Task.FromResult(_messageReturn);
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format(MessageLogErrors.Process, this.GetType().Name, nameof(PaymentDebitCard)), ex);
                throw;
            }
        }

        public Task<MessageReturn> GetStatusPayment(string paymentId)
        {
            try
            {
                Request request = new Request() { Data = paymentId };
                var url = Settings.PagbankStatusPayment + paymentId;
                Response response = new OperatorRest().SendRequestAsync(request, url, _config.Value.PagBankSettings.TokenHomolog);

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