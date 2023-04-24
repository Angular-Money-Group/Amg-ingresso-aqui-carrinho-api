using Amg_ingressos_aqui_carrinho_api.Model;

namespace Amg_ingressos_aqui_carrinho_api.Services.Interfaces
{
    public interface ITransactionService
    {
        Task<MessageReturn> SaveAsync(Transaction transaction);
        Task<MessageReturn> SaveTransactionItenAsync(Transaction transaction);
    }
}