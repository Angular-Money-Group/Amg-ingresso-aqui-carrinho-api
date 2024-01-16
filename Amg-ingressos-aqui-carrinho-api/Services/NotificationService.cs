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
        private MessageReturn _messageReturn;
        private readonly ILogger<NotificationService> _logger;

        public NotificationService(ILogger<NotificationService> logger)
        {
            _logger = logger;
            _messageReturn = new MessageReturn();
        }

        public async Task<MessageReturn> SaveAsync(NotificationEmailTicketDto notification)
        {
            _logger.LogInformation(string.Format("Init - Save: {0}",this.GetType().Name));
            try
            {
                var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Add(HeaderNames.Accept, "application/json");
                httpClient.Timeout = TimeSpan.FromMinutes(10);
                var jsonBody = new StringContent(JsonSerializer.Serialize(notification),
                Encoding.UTF8, Application.Json);
                var url = Settings.EmailServiceApi;
                var uri = Settings.UriEmailTicket;

                _logger.LogInformation(string.Format("Call PostAsync - Send: {0}",this.GetType().Name));
                var result = httpClient.PostAsync(url + uri, jsonBody);
                if(result.Result.IsSuccessStatusCode)
                {
                    _messageReturn.Message = "Success";
                }

                _logger.LogInformation(string.Format("Finished - Save: {0}",this.GetType().Name));
                return _messageReturn;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string GenerateBody()
        {
            try
            {
                _logger.LogInformation(string.Format("Init - GenerateBody: {0}",this.GetType().Name));
                var path  = (Environment.CurrentDirectory + "/Template/index.html");
                var html = System.IO.File.ReadAllText(path);
                var body = html;
                _logger.LogInformation(string.Format("Finished - GenerateBody: {0}",this.GetType().Name));
                return body;
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("error - GenerateBody: {0},message: {1}",this.GetType().Name,ex.Message),ex);
                throw ex;
            }
        }
    }
}