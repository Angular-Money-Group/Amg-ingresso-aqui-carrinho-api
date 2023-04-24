using Amg_ingressos_aqui_carrinho_api.Model;

namespace Amg_ingressos_aqui_carrinho_api.Services.Interfaces
{
    public interface ITransactionPaymentService
    {
        Task<MessageReturn> SaveAsync(TransactionPayment transaction);
        Task<MessageReturn> Payment(TransactionPayment transaction);
    }
}