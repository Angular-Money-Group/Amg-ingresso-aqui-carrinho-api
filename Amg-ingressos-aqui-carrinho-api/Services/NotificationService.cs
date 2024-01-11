using System.Text;
using System.Text.Json;
using Amg_ingressos_aqui_carrinho_api.Model;
using Amg_ingressos_aqui_carrinho_api.Repository.Interfaces;
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
        private HttpClient _HttpClient;
        private readonly ILogger<NotificationService> _logger;

        public NotificationService(IEmailRepository emailRepository,ILogger<NotificationService> logger)
        {
            _HttpClient = new HttpClient();
            _HttpClient.DefaultRequestHeaders.Add(HeaderNames.Accept, "application/json");
            _HttpClient.Timeout = TimeSpan.FromMinutes(10);
            _logger = logger;
            _messageReturn = new MessageReturn();
        }

        public async Task<MessageReturn> SaveAsync(NotificationEmailTicketDto notification)
        {
            _logger.LogInformation(string.Format("Init - Save: {0}",this.GetType().Name));
            try
            {
                _logger.LogInformation(string.Format("Save Repository - Save: {0}",this.GetType().Name));
                var jsonBody = new StringContent(JsonSerializer.Serialize(notification),
                Encoding.UTF8, Application.Json);
                var url = Settings.EmailServiceApi;
                var uri = Settings.UriEmailTicket;

                _logger.LogInformation(string.Format("Call PostAsync - Send: {0}",this.GetType().Name));
                _HttpClient.PostAsync(url + uri, jsonBody).Wait();

                _logger.LogInformation(string.Format("Finished - Save: {0}",this.GetType().Name));
                return _messageReturn;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public MessageReturn Send(string idEmail)
        {
            _logger.LogInformation(string.Format("Init - Send: {0}",this.GetType().Name));
            var jsonBody = new StringContent(JsonSerializer.Serialize(new {emailID = idEmail}),
             Encoding.UTF8, Application.Json); // using static System.Net.Mime.MediaTypeNames;
            var url = "http://api.ingressosaqui.com:3006/";
            var uri = "v1/email/";

            _logger.LogInformation(string.Format("Call PostAsync - Send: {0}",this.GetType().Name));
            _HttpClient.PostAsync(url + uri, jsonBody).Wait();

            _logger.LogInformation(string.Format("Finished - Send: {0}",this.GetType().Name));
            return _messageReturn;
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