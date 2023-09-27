namespace Amg_ingressos_aqui_carrinho_api.Repository.Interfaces
{
    public interface ITransactionRepository 
    {
        Task<object> Save<T>(object objectTransaction);
        Task<object> Update<T>(object objectTransaction);
        Task<object> Delete(string id);
        Task<object> GetById(string id);
        Task<object> GetByUser(string idUser);
        Task<object> GetByUserEvent(string idUser);
        Task<object> GetByUserTickets(string idUser, string idEvent);
        
    }
}