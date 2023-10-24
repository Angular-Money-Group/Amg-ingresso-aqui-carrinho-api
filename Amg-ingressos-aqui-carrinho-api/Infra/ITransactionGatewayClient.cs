using Amg_ingressos_aqui_carrinho_api.Model;
using MongoDB.Driver;

namespace Amg_ingressos_aqui_carrinho_api.Infra
{
    public interface ITransactionGatewayClient
    {
        Task<MessageReturn> PaymentPixAsync(Transaction transaction);
        Task<MessageReturn> PaymentDebitCardAsync(Transaction transaction);
        Task<MessageReturn> PaymentCreditCardAsync(Transaction transaction,User user);
        Task<MessageReturn> PaymentSlipAsync(Transaction transaction);
    }
}