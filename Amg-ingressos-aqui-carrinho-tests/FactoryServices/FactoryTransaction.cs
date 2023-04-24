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
                IdEvent = "6442dcb6523d52533aeb1ae4",
                IdPaymentMethod = "6442dcb6523d52533aeb1ae4",
                IdPerson = "6442dcb6523d52533aeb1ae4",
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
                    IdVariante = "6442dcb6523d52533aeb1ae4",
                    IdLote ="6442dcb6523d52533aeb1ae4",
                    HalfPrice = false,
                    TicketPrice = new decimal(150),
                    AmoutTicket =1
                },
                new TransactionIten(){
                    Id ="6442dcb6523d52533aeb1ae4",
                    IdTransaction = "6442dcb6523d52533aeb1ae4",
                    IdVariante = "6442dcb6523d52533aeb1ae4",
                    IdLote ="6442dcb6523d52533aeb1ae4",
                    HalfPrice = true,
                    TicketPrice = new decimal(75),
                    AmoutTicket =2
                },
            };
        }
    
    
    }
}