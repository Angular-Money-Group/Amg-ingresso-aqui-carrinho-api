using System.Text;
using System.Text.Json;
using Amg_ingressos_aqui_carrinho_api.Infra;
using Amg_ingressos_aqui_carrinho_api.Model;
using Amg_ingressos_aqui_carrinho_api.Services.Interfaces;
using Newtonsoft.Json;
using static System.Net.Mime.MediaTypeNames;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Amg_ingressos_aqui_carrinho_api.Services
{
    public class TicketService : ITicketService
    {
        private MessageReturn _messageReturn;
        private HttpClient _HttpClient;
        public TicketService(ICieloClient cieloClient){
            _HttpClient = cieloClient.CreateClient();
            _messageReturn = new Model.MessageReturn();
        }

        public async Task<MessageReturn> GetTicketsAsync(string idLote)
        {
            var url = new Uri("api.ingressosaqui.com.br:3002/v1/eventos/lote/" + idLote);
            using var httpResponseMessage = await _HttpClient.GetAsync(url);
            //var result = httpResponseMessage.EnsureSuccessStatusCode();
            string jsonContent = httpResponseMessage.Content.ReadAsStringAsync().Result;

             _messageReturn.Data = JsonConvert.DeserializeObject<List<Ticket>>(jsonContent);
             return _messageReturn;
        }
        public async Task<MessageReturn> UpdateTicketsAsync(Ticket ticket)
        {
            var transactionJson = new StringContent(JsonSerializer.Serialize(ticket),
            Encoding.UTF8, Application.Json); // using static System.Net.Mime.MediaTypeNames;

            using var httpResponseMessage =
                await _HttpClient.PostAsync("api.ingressosaqui.com.br:3002/v1/eventos/lote/" + ticket.Id,
                 transactionJson);
            
            string jsonContent = httpResponseMessage.Content.ReadAsStringAsync().Result;

            _messageReturn.Data = JsonConvert.DeserializeObject<List<Ticket>>(jsonContent);
            return _messageReturn;
        }
    }
}