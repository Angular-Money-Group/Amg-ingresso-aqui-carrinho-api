using Amg_ingressos_aqui_carrinho_api.Dto;
using Amg_ingressos_aqui_carrinho_api.Model;

namespace Amg_ingressos_aqui_carrinho_api.Services.Interfaces
{
    public interface ITransactionItenService
    {
        Task<MessageReturn> DeleteByIdTransaction(string idTransaction);
        Task<MessageReturn> Save(TransactionIten transactionItem);
        Task<MessageReturn> GetByIdTransaction(string idTransaction);
        Task<MessageReturn> ProcessSaveTransactionItens(string idTransaction, List<TransactionItenDto> transactionItensDto, List<Ticket> listTicket);
    }
}