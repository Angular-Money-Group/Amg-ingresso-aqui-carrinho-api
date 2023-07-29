using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amg_ingressos_aqui_carrinho_api.Model;

namespace Amg_ingressos_aqui_carrinho_tests.FactoryServices
{
    public static class FactoryPaymentService
    {
        internal static Ticket SimpleTransacionCreditCard()
        {
            return new Ticket()
            {
                Id = "6442dcb6523d52533aeb1ae4",
                IdLot = "6442dcb6523d52533aeb1ae4",
                IdUser = "6442dcb6523d52533aeb1ae4",
                isSold = true,
                Position = "A1",
                Value = new decimal(50)
            };
        }
        internal static Ticket SimpleTicketNotSold()
        {
            return new Ticket()
            {
                Id = "6442dcb6523d52533aeb1ae4",
                IdLot = "6442dcb6523d52533aeb1ae4",
                IdUser = "6442dcb6523d52533aeb1ae4",
                isSold = false,
                Position = "A1",
                Value = new decimal(50)
            };
        }
        internal static List<Ticket> SimpleListTicket()
        {
            return new List<Ticket>(){
                new Ticket()
                {
                    Id = "6442dcb6523d52533aeb1ae4",
                    IdLot = "6442dcb6523d52533aeb1ae4",
                    IdUser = "6442dcb6523d52533aeb1ae4",
                    isSold = true,
                    Position= "A1",
                    Value= new decimal(50)
                },
                new Ticket()
                {
                    Id = "6552dcb6523d52533aeb1ae4",
                    IdLot = "6552dcb6523d52533aeb1ae4",
                    IdUser = "6552dcb6523d52533aeb1ae4",
                    isSold = true,
                    Position= "A2",
                    Value= new decimal(50)
                }
            };
        }
        internal static List<Ticket> SimpleListTicketNotSold()
        {
            return new List<Ticket>(){
                new Ticket()
                {
                    Id = "6442dcb6523d52533aeb1ae4",
                    IdLot = "6442dcb6523d52533aeb1ae4",
                    IdUser = "6442dcb6523d52533aeb1ae4",
                    isSold = false,
                    Position= "A1",
                    Value= new decimal(50)
                },
                new Ticket()
                {
                    Id = "6552dcb6523d52533aeb1ae4",
                    IdLot = "6552dcb6523d52533aeb1ae4",
                    IdUser = "6552dcb6523d52533aeb1ae4",
                    isSold = false,
                    Position= "A2",
                    Value= new decimal(50)
                }
            };
        }
        internal static List<Ticket> SimpleListTicketNotSoldAndWithoutValue()
        {
            return new List<Ticket>(){
                new Ticket()
                {
                    Id = "6442dcb6523d52533aeb1ae4",
                    IdLot = "6442dcb6523d52533aeb1ae4",
                    IdUser = "6442dcb6523d52533aeb1ae4",
                    isSold = false,
                    Position= "A1",
                    Value= new decimal(0)
                },
                new Ticket()
                {
                    Id = "6552dcb6523d52533aeb1ae4",
                    IdLot = "6552dcb6523d52533aeb1ae4",
                    IdUser = "6552dcb6523d52533aeb1ae4",
                    isSold = false,
                    Position= "A2",
                    Value= new decimal(0)
                }
            };
        }

    }
}