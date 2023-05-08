using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Amg_ingressos_aqui_carrinho_api.Infra;
using Amg_ingressos_aqui_carrinho_api.Model;
using Amg_ingressos_aqui_carrinho_api.Services.Interfaces;
using static System.Net.Mime.MediaTypeNames;

namespace Amg_ingressos_aqui_carrinho_api.Services
{
    public class PaymentService : IPaymentService
    {
        private MessageReturn _messageReturn;
        private HttpClient _HttpClient;
        public PaymentService(ICieloClient cieloClient){
            _HttpClient = cieloClient.CreateClient();
            _messageReturn = new Model.MessageReturn();
        }
        public async Task<MessageReturn> Payment(Transaction transaction)
        {

            var transactionJson = new StringContent(JsonSerializer.Serialize(transaction),
            Encoding.UTF8, Application.Json); // using static System.Net.Mime.MediaTypeNames;

            using var httpResponseMessage =
                await _HttpClient.PostAsync("https://apisandbox.cieloecommerce.cielo.com.br/1/sales",
                 transactionJson);

            var result = httpResponseMessage.EnsureSuccessStatusCode();
            _messageReturn.Data = "Ticket criado";

            return _messageReturn;
        }
    }
}