using System.Text;
using Amg_ingressos_aqui_carrinho_api.Consts;
using Amg_ingressos_aqui_carrinho_api.Infra;
using Amg_ingressos_aqui_carrinho_api.Model;
using Amg_ingressos_aqui_carrinho_api.Model.Pagbank;
using Amg_ingressos_aqui_carrinho_api.Services.Interfaces;
using Microsoft.Extensions.Options;

namespace Amg_ingressos_aqui_carrinho_api.Services
{
    public class PagbankService : IPagbankService
    {
        private readonly MessageReturn _messageReturn;
        private readonly IOptions<PaymentSettings> _config;
        private readonly ILogger<PagbankService> _logger;

        public PagbankService(IOptions<PaymentSettings> config, ILogger<PagbankService> logger)
        {
            _config = config;
            _messageReturn = new MessageReturn();
            _logger = logger;
        }
        public async Task<MessageReturn> GenerateSession()
        {
            try
            {
                var type = new { type = "card" };
                //cria pedido e paga
                Request request = new Request() { Data = System.Text.Json.JsonSerializer.Serialize(type) };
                var url = _config.Value.PagBankSettings.UrlApiHomolog + "/public-keys";
                Response response = await Task.Run(() => new OperatorRest().SendRequestAsync(request, url, _config.Value.PagBankSettings.TokenHomolog));

                if (string.IsNullOrEmpty(response.Data))
                {
                    StringBuilder messagejson = new StringBuilder();
                    response.MessageJsonToModel<CallbackErrorMessagePagBank>().ErrorMessages
                    .ForEach(x =>
                    {
                        messagejson.Append(x.Code + " = " + x.ParameterName + " -- Error:" + x.Description + " : " + x.ParameterName);
                    });
                    response.Message = messagejson.ToString();
                    _messageReturn.Message = response.Message;
                }

                _messageReturn.Data = response.JsonToModel<SessionPagbank>();
                return _messageReturn;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, string.Format(MessageLogErrors.Process, this.GetType().Name, nameof(GenerateSession)));
                throw;
            }
        }
    }
}