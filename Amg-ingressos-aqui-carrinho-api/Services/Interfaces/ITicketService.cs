using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amg_ingressos_aqui_carrinho_api.Model;

namespace Amg_ingressos_aqui_carrinho_api.Services.Interfaces
{
    public interface ITicketService
    {
        Task<MessageReturn> GetTicketsAsync(string idLote);
        Task<MessageReturn> UpdateTicketsAsync(Ticket ticket);
    }
}