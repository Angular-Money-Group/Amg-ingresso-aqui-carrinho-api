namespace Amg_ingressos_aqui_carrinho_api.Repository.Interfaces
{
    public interface ITransactionPaymentRepository
    {
        Task<object> Save<T>(object transactionComplet);
    }
}