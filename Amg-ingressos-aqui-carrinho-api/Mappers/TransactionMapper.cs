using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amg_ingressos_aqui_carrinho_api.Dtos;
using Amg_ingressos_aqui_carrinho_api.Model;
using AutoMapper;

namespace Amg_ingressos_aqui_carrinho_api.Mappers
{
    public static class TransactionItemMapper
    {
        public static Transaction StageTicketDataDtoToTransaction(this StageTicketDataDto stageTicket)
        {
            return new Transaction()
            {
                Id = stageTicket.Id,
                Tax = stageTicket.Tax,
                Discount = stageTicket.Discount,
                Stage = Enum.StageTransactionEnum.TicketsData

            };
        }

        public static Transaction StagePaymentDataDtoToTransaction(this StagePaymentDataDto stagePayment)
        {
            return new Transaction()
            {
                Id = stagePayment.Id,
                PaymentMethod = new PaymentMethod(){ 
                    IdPaymentMethod = stagePayment.IdPaymentMethod,
                    Brand = stagePayment.Brand,
                    CardNumber = stagePayment.CardNumber,
                    ExpirationDate = stagePayment.ExpirationDate,
                    Holder = stagePayment.Holder,
                    SaveCard = stagePayment.SaveCard,
                    SecurityCode = stagePayment.SecurityCode,
                    TypePayment = stagePayment.TypePayment
                },
                Stage = Enum.StageTransactionEnum.PaymentData
            };
        }
    }
}