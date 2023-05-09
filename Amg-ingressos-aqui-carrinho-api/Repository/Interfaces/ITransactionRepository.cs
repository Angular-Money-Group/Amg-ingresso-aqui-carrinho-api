namespace Amg_ingressos_aqui_carrinho_api.Repository.Interfaces
{
    public interface ITransactionRepository 
    {
        Task<object> Save<T>(object transactionComplet);
        Task<object> Update<T>(object transactionComplet);
        Task<object> GetById(string idTransaction);
        Task<object> GetByPerson(string idPerson);
    }
}