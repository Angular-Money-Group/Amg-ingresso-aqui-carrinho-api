using Amg_ingressos_aqui_carrinho_api.Dto;
using Amg_ingressos_aqui_carrinho_api.Model;

namespace Amg_ingressos_aqui_carrinho_api.Services.Interfaces
{
    public interface ICheckoutService
    {
        Task<MessageReturn> ProcessSaveAsync(TransactionDto transactionDto);
        Task<MessageReturn> UpdateTransactionPersonDataAsync(string idTransaction);
        Task<MessageReturn> UpdateTransactionTicketsDataAsync(string idTransaction, StageTicketDataDto transactionDto);
        Task<MessageReturn> UpdateTransactionPaymentDataAsync(string idTransaction, PaymentMethod transactionDto);
        Task<MessageReturn> PaymentTransactionAsync(string idTransaction);
        Task<MessageReturn> FinishedTransactionAsync(string idTransaction);
        Task<MessageReturn> CancelTransactionAsync(string idTransaction);
        Task<MessageReturn> GeneratePixQRcode(Transaction transaction);
    }
}