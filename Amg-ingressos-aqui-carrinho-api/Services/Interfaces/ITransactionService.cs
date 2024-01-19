using Amg_ingressos_aqui_carrinho_api.Dto;
using Amg_ingressos_aqui_carrinho_api.Model;

namespace Amg_ingressos_aqui_carrinho_api.Services.Interfaces
{
    public interface ITransactionService
    {
        Task<MessageReturn> ProcessSaveAsync(TransactionDto transactionDto);
        Task<MessageReturn> UpdateAsync(Transaction transaction);
        Task<MessageReturn> GetByIdAsync(string id);
        Task<MessageReturn> GetStatusPixPaymentAsync(string paymentId);
        Task<MessageReturn> GetByUserActivesAsync(string idUser);
        Task<MessageReturn> GetByUserHistoryAsync(string idUser);
        Task<MessageReturn> GetByUserTicketEventDataAsync(string idUser, string idEvent);
        Task<MessageReturn> RefundPaymentPixAsync(string idTransaction, long? amount);
        Task<MessageReturn> Payment(Transaction transaction);
        Task<MessageReturn> GeneratePixQRcode(Transaction transaction);
        Task<MessageReturn> FinishedTransactionAsync(Transaction transaction);
        Task<MessageReturn> CancelTransaction(Transaction transaction);
    }
}