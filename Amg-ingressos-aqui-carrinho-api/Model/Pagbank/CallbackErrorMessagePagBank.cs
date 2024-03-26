using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace Amg_ingressos_aqui_carrinho_api.Model.Pagbank
{
    public class CallbackErrorMessagePagBank
    {
        public CallbackErrorMessagePagBank()
        {
            ErrorMessages = new List<ErrorMessage>();
        }

        [JsonProperty("Error_messages")]
        [JsonPropertyName("Error_messages")]
        public List<ErrorMessage> ErrorMessages { get; set; }
    }
}