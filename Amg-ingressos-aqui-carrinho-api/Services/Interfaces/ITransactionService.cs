using Amg_ingressos_aqui_carrinho_api.Model;

namespace Amg_ingressos_aqui_carrinho_api.Services.Interfaces
{
    public interface ITransactionService
    {
        
        Task<MessageReturn> UpdateAsync(Transaction transaction);
        Task<MessageReturn> GetByIdAsync(string id);
        Task<MessageReturn> GetByUserActivesAsync(string idUser);
        Task<MessageReturn> GetByUserHistoryAsync(string idUser);
        Task<MessageReturn> GetByUserTicketEventDataAsync(string idUser, string idEvent);
        Task<MessageReturn> SaveAsync(Transaction transaction);
    }
}