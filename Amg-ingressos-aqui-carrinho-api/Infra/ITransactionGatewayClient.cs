using Amg_ingressos_aqui_carrinho_api.Model;

namespace Amg_ingressos_aqui_carrinho_api.Infra
{
    public interface ITransactionGatewayClient
    {
        Task<MessageReturn> PaymentPixAsync(Transaction transaction, User user);
        Task<MessageReturn> PaymentDebitCardAsync(Transaction transaction, User user);
        Task<MessageReturn> PaymentCreditCardAsync(Transaction transaction, User user);
        Task<MessageReturn> PaymentSlipAsync(Transaction transaction, User user);
        Task<MessageReturn> GetStatusPayment(string paymentId);
    }
}