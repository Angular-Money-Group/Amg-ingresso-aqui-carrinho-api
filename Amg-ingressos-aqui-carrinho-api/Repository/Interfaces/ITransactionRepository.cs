namespace Amg_ingressos_aqui_carrinho_api.Repository.Interfaces
{
    public interface ITransactionRepository 
    {
        Task<object> Save<T>(object objectTransaction);
        Task<object> Update<T>(object objectTransaction);
        Task<object> Delete<T>(string id);
        Task<object> GetById(string id);
        Task<object> GetByUser(string idUser);
    }
}