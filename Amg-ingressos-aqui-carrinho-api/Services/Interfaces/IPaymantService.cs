using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amg_ingressos_aqui_carrinho_api.Model;
using Amg_ingressos_aqui_carrinho_api.Model.Querys;

namespace Amg_ingressos_aqui_carrinho_api.Services.Interfaces
{
    public interface IPaymentService
    {
        Task<MessageReturn> Payment(Transaction transaction);
        Task<MessageReturn> PaymentCieloPixAsync(Transaction transaction);
        string GetStatusPayment(string paymentId);
    }
}