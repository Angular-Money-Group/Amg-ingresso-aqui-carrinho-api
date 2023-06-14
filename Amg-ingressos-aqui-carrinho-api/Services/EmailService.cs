using Amg_ingressos_aqui_carrinho_api.Model;
using Amg_ingressos_aqui_carrinho_api.Repository.Interfaces;
using Amg_ingressos_aqui_carrinho_api.Services.Interfaces;

namespace Amg_ingressos_aqui_carrinho_api.Services
{
    public class EmailService : IEmailService
    {
        private MessageReturn _messageReturn;
        private IEmailRepository _emailRepository;

        public EmailService(IEmailRepository emailRepository)
        {
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