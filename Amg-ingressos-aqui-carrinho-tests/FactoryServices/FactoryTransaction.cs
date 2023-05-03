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
                IdPaymentMethod = "6442dcb6523d52533aeb1ae4",
                IdPerson = "6442dcb6523d52533aeb1ae4",
                Status = Amg_ingressos_aqui_carrinho_api.Enum.StatusPayment.Aproved,
                Tax = new decimal(10.0),
                TransactionItens = SimpleTransactionItens()
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
        internal static TransactionDto SimpleTransactionDto()
        {
            return new TransactionDto()
            {
                IdCustomer = "6442dcb6523d52533aeb1ae4",
                TransactionItensDto = SimpleTransactionItensDto()
            };
        }

        internal static List<TransactionItenDto> SimpleTransactionItensDto()
        {
            return new List<TransactionItenDto>()
            {
                new TransactionItenDto(){
                    IdLot ="6442dcb6523d52533aeb1ae4",
                    HalfPrice = false,
                    AmountTicket =5
                },
                new TransactionItenDto(){
                    IdLot ="6442dcb6523d52533aeb1ae4",
                    HalfPrice = true,
                    AmountTicket =5
                },
            };
        }
    }
}