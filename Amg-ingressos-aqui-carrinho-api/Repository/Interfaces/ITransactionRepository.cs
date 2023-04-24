namespace Amg_ingressos_aqui_carrinho_api.Repository.Interfaces
{
    public interface ITransactionRepository 
    {
        Task<object> Save<T>(object transactionComplet);
        Task<object> SaveTransactionIten<T>(object transactionComplet);
        Task<object> SaveTransactionPayment<T>(object transactionPayment);
    }
}