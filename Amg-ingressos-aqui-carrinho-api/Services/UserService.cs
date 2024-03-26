using Amg_ingressos_aqui_carrinho_api.Consts;
using Amg_ingressos_aqui_carrinho_api.Model;
using Amg_ingressos_aqui_carrinho_api.Services.Interfaces;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;

namespace Amg_ingressos_aqui_carrinho_api.Services
{
    public class UserService : IUserService
    {
        private readonly MessageReturn _messageReturn;
        private readonly HttpClient _HttpClient;
        private readonly ILogger<UserService> _logger;

        public UserService(ILogger<UserService> logger)
        {
            _HttpClient = new HttpClient();
            _HttpClient.DefaultRequestHeaders.Add(HeaderNames.Accept, "application/json");
            _messageReturn = new MessageReturn();
            _logger = logger;
        }

        public async Task<MessageReturn> FindByIdAsync(string idUser)
        {
            try
            {
                var url = Settings.UserServiceApi;
                var uri = Settings.UriGetUser;

                var response = await _HttpClient.GetAsync(url + uri + idUser);
                if (response.IsSuccessStatusCode)
                {
                    var jsonContent = await response.Content.ReadAsStringAsync();
                    _messageReturn.Data = JsonConvert.DeserializeObject<User>(jsonContent) ?? new User();
                }
                else
                    _messageReturn.Message = "Erro ao obter os dados do usuário.";

                return _messageReturn;
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format(MessageLogErrors.Process, this.GetType().Name, nameof(FindByIdAsync)), ex);
                throw;
            }
        }
    }
}