using System.Text;
using System.Text.Json;
using Amg_ingressos_aqui_carrinho_api.Model;
using Amg_ingressos_aqui_carrinho_api.Services.Interfaces;
using static System.Net.Mime.MediaTypeNames;
using Microsoft.Net.Http.Headers;
using Amg_ingressos_aqui_carrinho_api.Dto;
using Amg_ingressos_aqui_carrinho_api.Consts;

namespace Amg_ingressos_aqui_carrinho_api.Services
{
    public class NotificationService : INotificationService
    {
        private readonly MessageReturn _messageReturn;
        private readonly ILogger<NotificationService> _logger;

        public NotificationService(ILogger<NotificationService> logger)
        {
            _logger = logger;
            _messageReturn = new MessageReturn();
        }

        public async Task<MessageReturn> SaveAsync(NotificationEmailTicketDto notification)
        {
            _logger.LogInformation(string.Format("Init - Save: {0}", this.GetType().Name));
            try
            {
                var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Add(HeaderNames.Accept, "application/json");
                httpClient.Timeout = TimeSpan.FromMinutes(10);
                var jsonBody = new StringContent(JsonSerializer.Serialize(notification),
                Encoding.UTF8, Application.Json);
                var url = Settings.EmailServiceApi;
                var uri = Settings.UriEmailTicket;

                _logger.LogInformation(string.Format("Call PostAsync - Send: {0}", this.GetType().Name));
                var result = await httpClient.PostAsync(url + uri, jsonBody);
                if (result.IsSuccessStatusCode)
                {
                    _messageReturn.Message = "Success";
                }

                _logger.LogInformation(string.Format("Finished - Save: {0}", this.GetType().Name));
                return _messageReturn;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, string.Format(MessageLogErrors.Process, this.GetType().Name, nameof(SaveAsync)));
                throw;
            }
        }

        public async Task<bool> ProcessEmailTicketAsync(TicketUserDataDto ticketUserDto, TicketEventDataDto ticketEventDto, string urlQrCode, bool halfprice)
        {
            try
            {
                var notification = new NotificationEmailTicketDto()
                {
                    AddressEvent = ticketEventDto.@event.address.addressDescription
                        + " - "
                        + ticketEventDto.@event.address.number
                        + " - "
                        + ticketEventDto.@event.address.neighborhood
                        + " - "
                        + ticketEventDto.@event.address.city
                        + " - "
                        + ticketEventDto.@event.address.state,
                    EndDateEvent = ticketEventDto.@event.endDate.ToString(),
                    EventName = ticketEventDto.@event.name,
                    LocalEvent = ticketEventDto.@event.local,
                    Sender = "suporte@ingressosaqui.com",
                    StartDateEvent = ticketEventDto.@event.startDate.ToString(),
                    Subject = "Ingressos",
                    To = ticketUserDto.User.email,
                    TypeTicket = halfprice ? "Meia Entrada" : "Inteira",
                    UrlQrCode = urlQrCode,
                    UserName = ticketUserDto.User.name,
                    VariantName = ticketEventDto.variant.name,
                };

                _ = await SaveAsync(notification);

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, string.Format(MessageLogErrors.Process, this.GetType().Name, nameof(ProcessEmailTicketAsync)));
                throw;
            }
        }
    }
}