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
        public static Transaction StageTicketDataDtoToTransaction(this StageTicketDataDto stageTicket){
            return new Transaction(){
                Id = stageTicket.Id,
                Tax = stageTicket.Tax,
                Discount= stageTicket.Discount,
                Stage = Enum.StageTransactionEnum.TicketsData
                
            };
        }
        public static Transaction StagePersonDataDtoToTransaction(this StagePersonDataDto stagePerson){
            return new Transaction(){
                Id = stagePerson.Id,
                Stage = Enum.StageTransactionEnum.PersonData
            };
        }

        public static Transaction StagePaymentDataDtoToTransaction(this StagePaymentDataDto stagePayment){
            return new Transaction(){
                Id = stagePayment.Id,
                IdPaymentMethod = stagePayment.IdPaymentMethod,
                Stage = Enum.StageTransactionEnum.PaymentData
            };
        }
    }
}