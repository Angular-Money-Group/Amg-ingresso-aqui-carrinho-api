using System.Text;
using System.Text.Json;
using Amg_ingressos_aqui_carrinho_api.Infra;
using Amg_ingressos_aqui_carrinho_api.Model;
using Amg_ingressos_aqui_carrinho_api.Repository.Interfaces;
using Amg_ingressos_aqui_carrinho_api.Services.Interfaces;
using static System.Net.Mime.MediaTypeNames;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;

namespace Amg_ingressos_aqui_carrinho_api.Services
{
    public class UserService : IUserService
    {
        private MessageReturn _messageReturn;
        private HttpClient _HttpClient;
        private readonly ILogger<UserService> _logger;

        public UserService()
        {
            _HttpClient = new HttpClient();
            _HttpClient.DefaultRequestHeaders.Add(HeaderNames.Accept, "application/json");
            _messageReturn = new MessageReturn();
        }

        public async Task<MessageReturn> FindByIdAsync(string idUser)
        {
            this._messageReturn = new MessageReturn();

            try
            {
                var url = "http://api.ingressosaqui.com:3005/";
                var uri = "v1/user/";

                var response = await _HttpClient.GetAsync(url + uri + idUser);
                if (response.IsSuccessStatusCode)
                {
                    var jsonContent = await response.Content.ReadAsStringAsync();
                    var userData = JsonConvert.DeserializeObject<User>(jsonContent);

                    _messageReturn.Data = userData;
                }
                else
                {
                    _messageReturn.Message = "Erro ao obter os dados do usuário.";
                }

                return _messageReturn;
            }
            catch (Exception ex)
            {
                _messageReturn.Message = "Ocorreu um erro ao processar a solicitação.";
                return _messageReturn;
            }
        }
    }
}