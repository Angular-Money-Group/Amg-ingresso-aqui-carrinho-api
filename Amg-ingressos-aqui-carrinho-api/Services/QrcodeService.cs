using Amg_ingressos_aqui_carrinho_api.Consts;
using Amg_ingressos_aqui_carrinho_api.Services.Interfaces;

namespace Amg_ingressos_aqui_carrinho_api.Services
{
    public class QrCodeService : IQrCodeService
    {
        public async Task<string> GenerateQrCode(string idTicket)
        {
            HttpClient httpClient = new HttpClient();
            var url = Settings.QrCodeServiceApi;
            var uri = Settings.UriGenerateQrCode + idTicket;
            using var httpResponseMessage = await httpClient.GetAsync(url + uri);
            string jsonContent = System.Text.Json.JsonSerializer.Deserialize<string>(
                httpResponseMessage.Content.ReadAsStringAsync().Result
            ) ?? string.Empty;
            return jsonContent;
        }
    }
}