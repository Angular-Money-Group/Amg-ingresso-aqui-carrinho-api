namespace Amg_ingressos_aqui_carrinho_api.Repository.Interfaces
{
    public interface ITransactionItenRepository 
    {
        Task<object> Save<T>(object transaction);
    }
}