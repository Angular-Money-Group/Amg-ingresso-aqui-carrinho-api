using Amg_ingressos_aqui_carrinho_api.Dto;
using Amg_ingressos_aqui_carrinho_api.Model;

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
                    Installments = stagePayment.Installments,
                    EncryptedCard = stagePayment.EncryptedCard,
                    AuthenticationMethod = stagePayment.AuthenticationMethod
                },
                Stage = Enum.StageTransactionEnum.PaymentData
            };
        }
    }
}
