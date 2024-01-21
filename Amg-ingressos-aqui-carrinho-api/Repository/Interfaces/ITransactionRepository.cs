namespace Amg_ingressos_aqui_carrinho_api.Repository.Interfaces
{
    public interface ITransactionRepository 
    {
        Task<object> Save<T>(object objectTransaction);
        Task<object> Update<T>(object objectTransaction);
        Task<object> Delete(string id);
        Task<T> GetById<T>(string id);
        Task<List<T>> GetByUser<T>(string idUser);
        Task<List<T>> GetByUserEventData<T>(string idUser);
        Task<List<T>> GetByUserTicketData<T>(string idUser, string idEvent);
        
    }
}