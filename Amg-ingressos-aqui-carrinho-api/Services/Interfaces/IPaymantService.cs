using Amg_ingressos_aqui_carrinho_api.Model;

namespace Amg_ingressos_aqui_carrinho_api.Services.Interfaces
{
    public interface IPaymentService
    {
        Task<MessageReturn> Payment(Transaction transaction);
        Task<MessageReturn> PaymentCieloPixAsync(Transaction transaction);
        Task<MessageReturn> GetStatusPayment(string paymentId);
    }
}