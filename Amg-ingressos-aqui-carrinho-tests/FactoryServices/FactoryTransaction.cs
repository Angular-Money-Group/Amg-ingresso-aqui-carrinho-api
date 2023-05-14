using Amg_ingressos_aqui_carrinho_api.Dtos;
using Amg_ingressos_aqui_carrinho_api.Model;

namespace Amg_ingressos_aqui_carrinho_tests.FactoryServices
{
    public static class FactoryTransaction
    {
        internal static Transaction SimpleTransaction()
        {
            return new Transaction()
            {
                Id = "6442dcb6523d52533aeb1ae4",
                PaymentMethod = new PaymentMethod()
                {
                    IdPaymentMethod = "6442dcb6523d52533aeb1ae4",
                    Brand = "VISA",
                    CardNumber = "4551870000000183",
                    ExpirationDate = "12/2021",
                    SecurityCode = "SecurityCode",
                    Holder = "Teste Holder",
                    SaveCard = false,
                    TypePayment = Amg_ingressos_aqui_carrinho_api.Enum.TypePaymentEnum.CreditCard
                },
                IdPerson = "6442dcb6523d52533aeb1ae4",
                Status = Amg_ingressos_aqui_carrinho_api.Enum.StatusPaymentEnum.Aproved,
                Tax = new decimal(10.0),
                TransactionItens = SimpleTransactionItens()
            };
        }
        internal static Transaction SimpleTransactionPersonData()
        {
            return new Transaction()
            {
                Id = "6442dcb6523d52533aeb1ae4",
                Stage = Amg_ingressos_aqui_carrinho_api.Enum.StageTransactionEnum.PersonData
            };
        }
        internal static List<TransactionIten> SimpleTransactionItens()
        {
            return new List<TransactionIten>()
            {
                new TransactionIten(){
                    Id ="6442dcb6523d52533aeb1ae4",
                    IdTransaction = "6442dcb6523d52533aeb1ae4",
                    IdTicket = "6442dcb6523d52533aeb1ae4",
                    HalfPrice = false,
                    TicketPrice = new decimal(150)
                },
                new TransactionIten(){
                    Id ="6442dcb6523d52533aeb1ae4",
                    IdTransaction = "6442dcb6523d52533aeb1ae4",
                    IdTicket = "6442dcb6523d52533aeb1ae4",
                    HalfPrice = true,
                    TicketPrice = new decimal(75),
                },
            };
        }
        internal static TransactionDto SimpleTransactionDtoStageConfirm()
        {
            return new TransactionDto()
            {
                IdCustomer = "6442dcb6523d52533aeb1ae4",
                TransactionItensDto = SimpleListTransactionItensDto()
            };
        }

        internal static List<TransactionItenDto> SimpleListTransactionItensDto()
        {
            return new List<TransactionItenDto>()
            {
                new TransactionItenDto(){
                    IdLot ="6442dcb6523d52533aeb1ae4",
                    HalfPrice = false,
                    AmountTicket =1
                },
                new TransactionItenDto(){
                    IdLot ="6442dcb6523d52533aeb1ae4",
                    HalfPrice = true,
                    AmountTicket =1
                },
            };
        }
        internal static StageTicketDataDto SimpleStageTicketDataDTo()
        {
            return new StageTicketDataDto()
            {
                Discount = new decimal(0),
                Tax = new decimal(50)
            };
        }
        internal static StagePaymentDataDto SimpleStagePaymentDataDToCreditCard()
        {
            return new StagePaymentDataDto()
            {
                Id = "6442dcb6523d52533aeb1ae4",
                IdPaymentMethod = "6442dcb6523d52533aeb1ae4",
                Brand = "VISA",
                CardNumber = "4551870000000183",
                ExpirationDate = "12/2021",
                SecurityCode = "SecurityCode",
                Holder = "Teste Holder",
                SaveCard = false,
                TypePayment = Amg_ingressos_aqui_carrinho_api.Enum.TypePaymentEnum.CreditCard,
                Installments = 1
            };
        }
        internal static StagePaymentDataDto SimpleStagePaymentDataDToDebitCard()
        {
            return new StagePaymentDataDto()
            {
                Id = "6442dcb6523d52533aeb1ae4",
                IdPaymentMethod = "6442dcb6523d52533aeb1ae4",
                Brand = "VISA",
                CardNumber = "4551870000000183",
                ExpirationDate = "12/2021",
                SecurityCode = "SecurityCode",
                Holder = "Teste Holder",
                SaveCard = false,
                TypePayment = Amg_ingressos_aqui_carrinho_api.Enum.TypePaymentEnum.DebitCard,
                Installments = 1
            };
        }
        internal static Transaction SimpleTransactionCreditCard()
        {
            return new Transaction()
            {
                Id = "6442dcb6523d52533aeb1ae4",
                PaymentMethod = new PaymentMethod()
                {
                    IdPaymentMethod = "6442dcb6523d52533aeb1ae4",
                    Brand = "VISA",
                    CardNumber = "4551870000000183",
                    ExpirationDate = "12/2021",
                    SecurityCode = "123",
                    Holder = "Teste Holder",
                    SaveCard = false,
                    TypePayment = Amg_ingressos_aqui_carrinho_api.Enum.TypePaymentEnum.CreditCard,
                    Installments = 1
                },
                Discount = 0,
                IdPerson = "6442dcb6523d52533aeb1ae4",
                ReturnUrl="",
                Stage= Amg_ingressos_aqui_carrinho_api.Enum.StageTransactionEnum.PaymentTransaction,
                Status= Amg_ingressos_aqui_carrinho_api.Enum.StatusPaymentEnum.InProgress,
                Tax = 30
            };
        }
    }
}