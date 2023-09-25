using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amg_ingressos_aqui_carrinho_api.Dto;
using Amg_ingressos_aqui_carrinho_api.Model;
using Amg_ingressos_aqui_carrinho_api.Model.Querys;

namespace Amg_ingressos_aqui_carrinho_api.Mappers
{
    public static class TransactionItemMapper
    {
        public static Transaction StageTicketDataDtoToTransaction(
            this StageTicketDataDto stageTicket
        )
        {
            return new Transaction()
            {
                Tax = stageTicket.Tax,
                Discount = stageTicket.Discount,
                Stage = Enum.StageTransactionEnum.TicketsData
            };
        }

        public static Transaction StagePaymentDataDtoToTransaction(this PaymentMethod stagePayment)
        {
            return new Transaction()
            {
                PaymentMethod = new PaymentMethod()
                {
                    IdPaymentMethod = stagePayment.IdPaymentMethod,
                    Brand = stagePayment.Brand,
                    CardNumber = stagePayment.CardNumber,
                    ExpirationDate = stagePayment.ExpirationDate,
                    Holder = stagePayment.Holder,
                    SaveCard = stagePayment.SaveCard,
                    SecurityCode = stagePayment.SecurityCode,
                    TypePayment = stagePayment.TypePayment,
                    Installments = stagePayment.Installments
                },
                Stage = Enum.StageTransactionEnum.PaymentData
            };
        }

        public static Transaction StagePaymentPixDataDtoToTransaction(
            this PaymentMethodPix stagePayment
        )
        {
            return new Transaction()
            {
                PaymentMethodPix = new PaymentMethodPix()
                {
                    MerchantOrderId = stagePayment.MerchantOrderId,
                    Customer = stagePayment.Customer,
                    Payment = stagePayment.Payment
                },
                Stage = Enum.StageTransactionEnum.PaymentData
            };
        }

        public static Transaction GeTransactionToTransaction(this GetTransaction getTransaction)
        {
            return new Transaction()
            {
                Id = getTransaction._id,
                IdPerson = getTransaction.IdPerson,
                PaymentMethod = new PaymentMethod()
                {
                    IdPaymentMethod = getTransaction.PaymentMethod.IdPaymentMethod,
                    Brand = getTransaction.PaymentMethod.Brand,
                    CardNumber = getTransaction.PaymentMethod.CardNumber,
                    ExpirationDate = getTransaction.PaymentMethod.ExpirationDate,
                    Holder = getTransaction.PaymentMethod.Holder,
                    SaveCard = getTransaction.PaymentMethod.SaveCard,
                    SecurityCode = getTransaction.PaymentMethod.SecurityCode,
                    TypePayment = getTransaction.PaymentMethod.TypePayment,
                    Installments = getTransaction.PaymentMethod.Installments
                },
                Stage = getTransaction.Stage,
                Discount = getTransaction.Discount,
                PaymentIdService = getTransaction.PaymentIdService,
                ReturnUrl = getTransaction.ReturnUrl,
                Status = getTransaction.Status,
                Tax = getTransaction.Tax,
                TotalValue = getTransaction.TotalValue,
                Details = getTransaction.Details
            };
        }

        public static Transaction GeTransactionToPixTransaction(this GetTransaction getTransaction)
        {
            return new Transaction()
            {
                Id = getTransaction._id,
                IdPerson = getTransaction.IdPerson,
                PaymentMethodPix = new PaymentMethodPix()
                {
                    MerchantOrderId = getTransaction.PaymentMethodPix.MerchantOrderId,
                    Customer = getTransaction.PaymentMethodPix.Customer,
                    Payment = getTransaction.PaymentMethodPix.Payment
                },
                PaymentPix = getTransaction.PaymentPix,
                Stage = getTransaction.Stage,
                Discount = getTransaction.Discount,
                PaymentIdService = getTransaction.PaymentIdService,
                ReturnUrl = getTransaction.ReturnUrl,
                Status = getTransaction.Status,
                Tax = getTransaction.Tax,
                TotalValue = getTransaction.TotalValue,
                Details = getTransaction.Details
            };
        }


        public static Transaction GetTransactionPixToTransactionPix(this GetTransaction getTransaction)
        {
            return new Transaction()
            {
                Id = getTransaction._id,
                IdPerson = getTransaction.IdPerson,
                PaymentMethodPix = getTransaction.PaymentMethodPix,
                Stage = getTransaction.Stage,
                Discount = getTransaction.Discount,
                PaymentIdService = getTransaction.PaymentIdService,
                ReturnUrl = getTransaction.ReturnUrl,
                Status = getTransaction.Status,
                Tax = getTransaction.Tax,
                TotalValue = getTransaction.TotalValue,
                Details = getTransaction.Details
            };
        }
    }
}
