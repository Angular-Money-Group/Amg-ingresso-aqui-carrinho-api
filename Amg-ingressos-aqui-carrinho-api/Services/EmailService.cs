using System.Text;
using System.Text.Json;
using Amg_ingressos_aqui_carrinho_api.Infra;
using Amg_ingressos_aqui_carrinho_api.Model;
using Amg_ingressos_aqui_carrinho_api.Repository.Interfaces;
using Amg_ingressos_aqui_carrinho_api.Services.Interfaces;
using static System.Net.Mime.MediaTypeNames;

namespace Amg_ingressos_aqui_carrinho_api.Services
{
    public class EmailService : IEmailService
    {
        private MessageReturn _messageReturn;
        private IEmailRepository _emailRepository;
        private HttpClient _HttpClient;

        public EmailService(IEmailRepository emailRepository,ICieloClient cieloClient)
        {
            _HttpClient = cieloClient.CreateClient();
            _emailRepository = emailRepository;
            _messageReturn = new MessageReturn();
        }

        public async Task<MessageReturn> SaveAsync(Email email)
        {
            try
            {
                _messageReturn.Data = await _emailRepository.SaveAsync(email);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return _messageReturn;
        }
        public MessageReturn Send(string idEmail)
        {
            var ticketJson = new StringContent(JsonSerializer.Serialize(new {emailID = idEmail}),
             Encoding.UTF8, Application.Json); // using static System.Net.Mime.MediaTypeNames;
            var url = "https://api.ingressosaqui.com/";
            var uri = "v1/email/";

            _HttpClient.PostAsync(url + uri, ticketJson).Wait();
            return _messageReturn;
        }

        public string GenerateBody()
        {
            try
            {
                var html = System.IO.File.ReadAllText(@"Template/index.html");
                var body = html;
                return body;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}