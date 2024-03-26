using Amg_ingressos_aqui_carrinho_api.Dto;
using Amg_ingressos_aqui_carrinho_api.Model;

namespace Amg_ingressos_aqui_carrinho_api.Services.Interfaces
{
    public interface INotificationService
    {
        Task<MessageReturn> SaveAsync(NotificationEmailTicketDto notification);
        Task<bool> ProcessEmailTicketAsync(TicketUserDataDto ticketUserDto, TicketEventDataDto ticketEventDto,string urlQrCode, bool halfprice);
    }
}