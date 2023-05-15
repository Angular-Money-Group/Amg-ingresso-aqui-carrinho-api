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
        public TicketService(ICieloClient cieloClient)
        {
            _HttpClient = cieloClient.CreateClient();
            _messageReturn = new Model.MessageReturn();
        }

        public async Task<MessageReturn> GetTicketsByLotAsync(string idLote)
        {
            //var url = new Uri(@);
            var url = "http://172.17.0.2:80/";
            var uri = "v1/tickets/lote/" + idLote;
            using var httpResponseMessage = await _HttpClient
                .GetAsync(url + uri);
            //var result = httpResponseMessage.EnsureSuccessStatusCode();
            string jsonContent = httpResponseMessage.Content.ReadAsStringAsync().Result;

            _messageReturn.Data = JsonConvert.DeserializeObject<List<Ticket>>(jsonContent);
            return _messageReturn;
        }
        public async Task<MessageReturn> GetTicketsByIdAsync(string id)
        {
            //var url = new Uri(@);
            var url = "http://172.17.0.2:80/";
            var uri = "v1/tickets/" + id;
            using var httpResponseMessage = await _HttpClient
                .GetAsync(url + uri);
            //var result = httpResponseMessage.EnsureSuccessStatusCode();
            string jsonContent = httpResponseMessage.Content.ReadAsStringAsync().Result;

            _messageReturn.Data = JsonConvert.DeserializeObject<Ticket>(jsonContent);
            return _messageReturn;
        }

        public async Task<MessageReturn> UpdateTicketsAsync(Ticket ticket)
        {
            try
            {
                var ticketJson = new StringContent(JsonSerializer.Serialize(ticket),
            Encoding.UTF8, Application.Json); // using static System.Net.Mime.MediaTypeNames;
                var url = "http://api.ingressosaqui.com:3002/";
                var uri = "v1/tickets/" + ticket.Id;

                //using var httpResponseMessage =
                    _HttpClient.PutAsync(url + uri, ticketJson).Wait();

                //string jsonContent = httpResponseMessage.Content.ReadAsStringAsync().Result;

                //_messageReturn.Data = JsonConvert.DeserializeObject<List<Ticket>>(jsonContent);
                return _messageReturn;
            }
            catch (System.Exception ex)
            {
                throw;
            }
        }
    }
}