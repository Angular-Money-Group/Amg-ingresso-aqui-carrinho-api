using Amg_ingressos_aqui_carrinho_api.Model;

namespace Amg_ingressos_aqui_carrinho_api.Repository.Interfaces
{
    public interface ITransactionItenRepository 
    {
        Task<TransactionIten> Save(TransactionIten transaction);
        Task<bool> DeleteByIdTransaction(string idTransaction);
        Task<List<T>> GetByIdTransaction<T>(string idTransaction);
    }
}