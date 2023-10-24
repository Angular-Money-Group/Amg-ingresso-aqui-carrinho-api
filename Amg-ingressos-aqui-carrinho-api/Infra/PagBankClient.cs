using System.Text;
using Amg_ingressos_aqui_carrinho_api.Model;
using Amg_ingressos_aqui_carrinho_api.Enum;
using Newtonsoft.Json;
using static System.Net.Mime.MediaTypeNames;
using Amg_ingressos_aqui_carrinho_api.Exceptions;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using Amg_ingressos_aqui_carrinho_api.Dto.Pagbank;
using Amg_ingressos_aqui_carrinho_api.Model.Pagbank;

namespace Amg_ingressos_aqui_carrinho_api.Infra
{
    public class PagBankClient : ITransactionGatewayClient
    {
        private IOptions<PaymentSettings> _config;
        private MessageReturn _messageReturn;
        private HttpClient _httpClient = new HttpClient();
        public PagBankClient(IOptions<PaymentSettings> transactionDatabaseSettings)
        {
            _config = transactionDatabaseSettings;
            _messageReturn = new MessageReturn();
            CreateClient();
        }

        private HttpClient CreateClient()
        {
            _httpClient.BaseAddress = new Uri(_config.Value.PagBankSettings.UrlApiHomolog);
            _httpClient.DefaultRequestHeaders.Add(
                HeaderNames.Accept, "application/json");
            _httpClient.DefaultRequestHeaders.Add(
                "Authorization", _config.Value.PagBankSettings.TokenHomolog);
            _httpClient.Timeout = TimeSpan.FromMinutes(10);

            return _httpClient;
        }

        public Task<MessageReturn> PaymentSlipAsync(Transaction transaction)
        {
            throw new NotImplementedException();
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
                Request request = new RequestPagBankDto().TransactionToRequest(transaction, user);
                Response response = SendRequestAsync(request);

                if(!string.IsNullOrEmpty(response.Data)){
                    var obj = JsonConvert.DeserializeObject<CallbackCreditCardPagBank>(response.Data);
                    transaction.PaymentIdService = obj.id;
                    _messageReturn.Data = "ok";
                }
                else{
                    _messageReturn.Message = response.Message;
                    transaction.Status = StatusPaymentEnum.ErrorPayment;
                    transaction.Details = response.Message;
                }
            }
            catch (System.Exception)
            {
                throw;
            }

            return _messageReturn;
        }

        private string? TrataErroRetorno(HttpResponseMessage response)
        {
            string jsonContent = response.Content.ReadAsStringAsync().Result;
            var obj = JsonConvert.DeserializeObject<CallbackErrorMessagePagBank>(response.ToString());
                _messageReturn.Message = Consts.StatusCallbackCielo.NotAllowed;
                
            return obj.ToString();
            //valida retorno
            /*switch (StatusCallbackCielo.SuccessfullyPerformedOperation)//obj.reference_id)
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
            }*/
        }

        public Task<MessageReturn> PaymentDebitCardAsync(Transaction transaction)
        {
            throw new NotImplementedException();
        }

        public Task<MessageReturn> PaymentPixAsync(Transaction transaction)
        {
            throw new NotImplementedException();
        }

        private async Task<bool> ValidateCard(Transaction transactionJson)
        {
            Request requestCartao = new Request
                {
                    Data = System.Text.Json.JsonSerializer.Serialize(
                     new PaymentCard()
                     {
                         number = "5590800827578129",
                         exp_month = "04",
                         exp_year = "2024",
                         security_code = "123",
                         holder = new Model.Holder()
                         {
                             name = "Jose da Silva"
                         }
                     })
                };

            try
            {
                var requestMessage = new HttpRequestMessage(
                    HttpMethod.Post,
                    "https://sandbox.api.pagseguro.com/tokens/cards"
                );
                requestMessage.Headers.Add("Accept", "*/*");
                requestMessage.Headers.Add("Accept-Encoding", "gzip, deflate, br");
                requestMessage.Headers.Add("Connection", "keep-alive");
                requestMessage.Headers.Add("Authorization", _config.Value.PagBankSettings.TokenHomolog);
                var requestJson = new StringContent(requestCartao.Data,
                    Encoding.UTF8,
                    Application.Json
                );
                requestMessage.Content = requestJson;

                using var httpResponseMessage = _httpClient.Send(requestMessage);

                string jsonContent = httpResponseMessage.Content.ReadAsStringAsync().Result;

                var result = httpResponseMessage.EnsureSuccessStatusCode();

                var response = new Response();
                if(httpResponseMessage.StatusCode == System.Net.HttpStatusCode.OK)
                    return true;
                else
                    return false;

            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        private Response SendRequestAsync(Request transactionJson)
        {
            try
            {
                var httpCliente = new HttpClient();
                var requestMessage = new HttpRequestMessage(
                    HttpMethod.Post,
                    "https://sandbox.api.pagseguro.com/orders"
                );
                requestMessage.Headers.Add("Accept", "*/*");
                //requestMessage.Headers.Add("Accept-Encoding", "gzip, deflate, br");
                //requestMessage.Headers.Add("Content-Type", "application/json");
                requestMessage.Headers.Add("Authorization", _config.Value.PagBankSettings.TokenHomolog);
                var requestJson = new StringContent(transactionJson.Data,
                    Encoding.UTF8,
                    Application.Json
                );
                requestMessage.Content = requestJson;

                using var httpResponseMessage = httpCliente.Send(requestMessage);
                string jsonContent = httpResponseMessage.Content.ReadAsStringAsync().Result;
                var response = new Response();
                if(httpResponseMessage.StatusCode == System.Net.HttpStatusCode.OK){
                    
                    var messagejson= JsonConvert.DeserializeObject<CallbackCreditCardPagBank>(jsonContent).ToString();
                    response.Data= messagejson;
                }
                else{
                    var messagejson = new StringBuilder();
                    JsonConvert.DeserializeObject<CallbackErrorMessagePagBank>(jsonContent).Error_messages.ForEach(x=> {
                        messagejson.Append(x.code+" = "+ x.parameter_name +" -- Error:"+ x.description +" : "+ x.parameter_name );
                    });
                    response.Message=messagejson.ToString();
                }

                return response;
                
            }
            catch (System.Exception ex)
            {
                throw;
            }
        }
    }
}