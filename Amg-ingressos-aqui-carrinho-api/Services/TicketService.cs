using System.Text;
using Amg_ingressos_aqui_carrinho_api.Consts;
using Amg_ingressos_aqui_carrinho_api.Dto;
using Amg_ingressos_aqui_carrinho_api.Exceptions;
using Amg_ingressos_aqui_carrinho_api.Model;
using Amg_ingressos_aqui_carrinho_api.Services.Interfaces;
using MongoDB.Bson;
using Newtonsoft.Json;
using static System.Net.Mime.MediaTypeNames;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Amg_ingressos_aqui_carrinho_api.Services
{
    public class TicketService : ITicketService
    {
        private readonly HttpClient _httpClient = new HttpClient();
        private readonly ILogger<TicketService> _logger;
        public TicketService(ILogger<TicketService> logger)
        {
            _logger = logger;
        }

        public async Task<List<Ticket>> GetTicketsByLotAsync(string idLote)
        {
            var url = Settings.TicketServiceApi;
            var uri = Settings.UriTicketsLot + idLote;
            using var httpResponseMessage = await _httpClient.GetAsync(url + uri);
            string jsonContent = httpResponseMessage.Content.ReadAsStringAsync().Result;

            return JsonConvert.DeserializeObject<List<Ticket>>(jsonContent) ?? throw new RuleException("erro ao buscar tickets.");
        }

        public async Task<TicketUserDataDto> GetTicketByIdDataUserAsync(string id)
        {
            var url = Settings.TicketServiceApi;
            var uri = string.Format(Settings.UriGetTicketDataUser, id);
            using var httpResponseMessage = await _httpClient.GetAsync(url + uri);
            string jsonContent = httpResponseMessage.Content.ReadAsStringAsync().Result;

            return JsonConvert.DeserializeObject<TicketUserDataDto>(jsonContent) ?? throw new RuleException("erro ao buscar tickets.");
        }

        public async Task<TicketEventDataDto> GetTicketByIdDataEventAsync(string id)
        {
            var url = Settings.TicketServiceApi;
            var uri = string.Format(Settings.UriGetTicketDataEvent, id);
            using var httpResponseMessage = await _httpClient.GetAsync(url + uri);
            string jsonContent = httpResponseMessage.Content.ReadAsStringAsync().Result;

            return JsonConvert.DeserializeObject<TicketEventDataDto>(jsonContent) ?? throw new RuleException("erro ao buscar tickets.");
        }

        public async Task<bool> EditTicketsAsync(Ticket ticket)
        {
            var ticketJson = new StringContent(JsonSerializer.Serialize(ticket), Encoding.UTF8, Application.Json);
            var url = Settings.TicketServiceApi;
            var uri = string.Format(Settings.UriUpdateTicket, ticket.Id);

            var result = await _httpClient.PutAsync(url + uri, ticketJson);
            if (result.IsSuccessStatusCode)
                return true;
            else
                return false;
        }

        public Task<List<Ticket>> ReservTicketsAsync(TransactionDto transactionDto)
        {
            try
            {
                var listTicket = new List<Ticket>();
                transactionDto.TransactionItensDto.ForEach(i =>
                {
                    //retorna todos tickets para o idLote
                    var lstTickets = GetTicketsByLotAsync(i.IdLot).Result;

                    if (lstTickets == null)
                        throw new RuleException("Erro ao buscar Ingressos");

                    if (!lstTickets.Any() && lstTickets.Count < i.AmountTicket)
                        throw new SaveException("Número de ingressos inválido");

                    //pra cada compra carimbar o ticket
                    for (int amount = 0; amount < i.AmountTicket; amount++)
                    {
                        Ticket ticket = lstTickets.Find(i => !i.IsSold) ?? throw new RuleException("ticket não encontrado.");

                        if (ticket.Value <= 0)
                            throw new SaveException("Valor do Ingresso inválido.");

                        //atualiza Ticket
                        ticket.IdUser = transactionDto.IdUser;
                        ticket.Status = Enum.StatusTicket.Reservado;
                        if (!EditTicketsAsync(ticket).Result)
                            throw new RuleException("Erro ao atualizar ticket");
                        
                        listTicket.Add(ticket);
                        //remove ticket da lista para proximo processamento
                        lstTickets.Remove(ticket);
                    }
                });

                return Task.FromResult(listTicket);
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format(MessageLogErrors.Process, this.GetType().Name, nameof(ReservTicketsAsync)), ex);
                throw;
            }
        }
    }
}