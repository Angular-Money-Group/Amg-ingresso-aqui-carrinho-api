using Amg_ingressos_aqui_carrinho_api.Model;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;

namespace Amg_ingressos_aqui_carrinho_api.Infra
{
    public class CieloClient : ITransactionGatewayClient
    {
        private readonly HttpClient _httpClient;
        private IOptions<PaymentSettings> _config;
        public CieloClient(IOptions<PaymentSettings> transactionDatabaseSettings, HttpClient httpClientFactory)
        {
            _config = transactionDatabaseSettings;
            _httpClient = httpClientFactory;
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

        public Task<MessageReturn> PaymentCreditCardAsync(Transaction transaction)
        {
            throw new NotImplementedException();
        }

        public Task<MessageReturn> PaymentCreditCardAsync(Transaction transaction, User user)
        {
            throw new NotImplementedException();
        }

        public Task<MessageReturn> PaymentDebitCardAsync(Transaction transaction)
        {
            throw new NotImplementedException();
        }

        public Task<MessageReturn> PaymentPixAsync(Transaction transaction)
        {
            throw new NotImplementedException();
        }

        public Task<MessageReturn> PaymentSlipAsync(Transaction transaction)
        {
            throw new NotImplementedException();
        }
    }
}