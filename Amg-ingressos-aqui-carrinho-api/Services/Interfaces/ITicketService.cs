using Amg_ingressos_aqui_carrinho_api.Dto;
using Amg_ingressos_aqui_carrinho_api.Model;

namespace Amg_ingressos_aqui_carrinho_api.Services.Interfaces
{
    public interface ITicketService
    {
        Task<List<Ticket>> GetTicketsByLotAsync(string idLote);
        Task<bool> UpdateTicketsAsync(Ticket ticket);
        Task<TicketUserDataDto> GetTicketByIdDataUserAsync(string id);
        Task<TicketEventDataDto> GetTicketByIdDataEventAsync(string id);
        Task<List<Ticket>> ReservTicketsAsync(TransactionDto transactionDto);
    }
}