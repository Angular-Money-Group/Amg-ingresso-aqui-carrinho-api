using Amg_ingressos_aqui_carrinho_api.Model;

namespace Amg_ingressos_aqui_carrinho_api.Infra
{
    public interface ITransactionGatewayClient
    {
        Task<MessageReturn> PaymentPix(Transaction transaction, User user);
        Task<MessageReturn> PaymentDebitCard(Transaction transaction, User user);
        Task<MessageReturn> PaymentCreditCard(Transaction transaction, User user);
        Task<MessageReturn> PaymentSlip(Transaction transaction, User user);
        Task<MessageReturn> GetStatusPayment(string paymentId);
    }
}