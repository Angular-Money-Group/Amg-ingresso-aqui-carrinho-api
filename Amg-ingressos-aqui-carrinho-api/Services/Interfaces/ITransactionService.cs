using Amg_ingressos_aqui_carrinho_api.Dtos;
using Amg_ingressos_aqui_carrinho_api.Model;

namespace Amg_ingressos_aqui_carrinho_api.Services.Interfaces
{
    public interface ITransactionService
    {
        Task<MessageReturn> SaveAsync(TransactionDto transaction);
        Task<MessageReturn> SaveTransactionItenAsync(string IdTransaction,string IdCustomer, List<TransactionItenDto> transactionItens);
        Task<MessageReturn> UpdateAsync(Transaction transaction);
        Task<MessageReturn> GetByIdAsync(string idTransaction);
        Task<MessageReturn> Payment(Transaction transaction);
        Task<MessageReturn> FinishedTransactionAsync(Transaction transaction);
    }
}