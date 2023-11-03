using System.Text;
using Amg_ingressos_aqui_carrinho_api.Infra;
using Amg_ingressos_aqui_carrinho_api.Model;
using Amg_ingressos_aqui_carrinho_api.Model.Pagbank;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Amg_ingressos_aqui_carrinho_api.Services.Interfaces
{
    public class PagbankService : IPagbankService
    {
        private MessageReturn _messageReturn;
        private IOptions<PaymentSettings> _config;

        public PagbankService(IOptions<PaymentSettings> config)
        {
            _config = config;
            _messageReturn = new Model.MessageReturn();
        }
        public async Task<MessageReturn> GenerateSession()
        {
            try
            {
                var type = new { type = "card" };
                //cria pedido e paga
                Request request = new Request() { Data = System.Text.Json.JsonSerializer.Serialize(type) };
                var url =  _config.Value.PagBankSettings.UrlApiHomolog + "/public-keys";
                Response response = new OperatorRest().SendRequestAsync(request,url,_config.Value.PagBankSettings.TokenHomolog);

                if (!string.IsNullOrEmpty(response.Data))
                {
                    var obj = JsonConvert.DeserializeObject<SessionPagbank>(response.Data);
                    _messageReturn.Data = obj;
                }
                else
                {
                    StringBuilder messagejson = new StringBuilder();
                    JsonConvert.DeserializeObject<CallbackErrorMessagePagBank>(response.Message).Error_messages.ForEach(x =>
                    {
                        messagejson.Append(x.code + " = " + x.parameter_name + " -- Error:" + x.description + " : " + x.parameter_name);
                    });
                    response.Message = messagejson.ToString();
                    _messageReturn.Message = response.Message;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return _messageReturn;
        }
    }
}