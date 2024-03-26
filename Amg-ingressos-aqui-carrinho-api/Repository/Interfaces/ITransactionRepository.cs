using Amg_ingressos_aqui_carrinho_api.Model;

namespace Amg_ingressos_aqui_carrinho_api.Repository.Interfaces
{
    public interface ITransactionRepository 
    {
        Task<Transaction> Save(Transaction transaction);
        Task<bool> Edit( string id,Transaction transaction);
        Task<bool> Delete(string id);
        Task<T> GetById<T>(string id);
        Task<List<T>> GetByUser<T>(string idUser);
        Task<List<T>> GetByUserEventData<T>(string idUser);
        Task<List<T>> GetByUserTicketData<T>(string idUser, string idEvent);   
    }
}