using Amg_ingressos_aqui_carrinho_api.Model;

namespace Amg_ingressos_aqui_carrinho_api.Services.Interfaces
{
    public interface ITransactionItenService
    {
        Task<bool> DeleteByIdTransaction(string idTransaction);
        Task<TransactionIten> Save(TransactionIten transactionItem);
        Task<List<T>>GetByIdTransaction<T>(string idTransaction);
    }
}