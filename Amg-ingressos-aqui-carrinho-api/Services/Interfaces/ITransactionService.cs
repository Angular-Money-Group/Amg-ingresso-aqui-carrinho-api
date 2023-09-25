using Amg_ingressos_aqui_carrinho_api.Dto;
using Amg_ingressos_aqui_carrinho_api.Model;

namespace Amg_ingressos_aqui_carrinho_api.Services.Interfaces
{
    public interface ITransactionService
    {
        Task<MessageReturn> SaveAsync(TransactionDto transaction);
        Task<MessageReturn> SaveTransactionItenAsync(string IdTransaction,string IdUser, List<TransactionItenDto> transactionItens);
        Task<MessageReturn> UpdateAsync(Transaction transaction);
        Task<MessageReturn> GetByIdAsync(string idTransaction);
        Task<MessageReturn> GetStatusPixPaymentAsync(string paymentId);
        Task<MessageReturn> GetByUserAsync(string idUser);
        Task<MessageReturn> GetByUserEventAsync(string idUser, string? idEvent);
        Task<MessageReturn> RefundPaymentPixAsync(string idTransaction, long? amount);
        Task<MessageReturn> Payment(Transaction transaction);
        Task<MessageReturn> GeneratePixQRcode(Transaction transaction);
        Task<MessageReturn> FinishedTransactionAsync(Transaction transaction);
        Task<MessageReturn> CancelTransaction(Transaction transaction);
    }
}