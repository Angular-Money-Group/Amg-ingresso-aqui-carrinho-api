using System.Text;
using Amg_ingressos_aqui_carrinho_api.Model;
using static System.Net.Mime.MediaTypeNames;

namespace Amg_ingressos_aqui_carrinho_api.Infra
{
    public class OperatorRest
    {

        public Response SendRequestAsync(Request transactionJson, string urlServer, string Token)
        {

            var httpCliente = new HttpClient();
            var requestMessage = new HttpRequestMessage(
                HttpMethod.Post,
                urlServer
            );
            requestMessage.Headers.Add("Accept", "*/*");
            requestMessage.Headers.Add("Authorization", Token);
            var requestJson = new StringContent(transactionJson.Data,
                Encoding.UTF8,
                Application.Json
            );
            requestMessage.Content = requestJson;

            using var httpResponseMessage = httpCliente.Send(requestMessage);
            string jsonContent = httpResponseMessage.Content.ReadAsStringAsync().Result;
            var response = new Response();

            if (httpResponseMessage.StatusCode == System.Net.HttpStatusCode.OK || httpResponseMessage.StatusCode == System.Net.HttpStatusCode.Created)
            {
                response.Data = jsonContent;
            }
            else
            {
                response.Message = jsonContent;
            }

            return response;
        }
    }
}