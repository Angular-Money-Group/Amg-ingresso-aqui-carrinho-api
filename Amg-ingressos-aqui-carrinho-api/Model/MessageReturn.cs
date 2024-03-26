using System.Text.Json.Serialization;
using Amg_ingressos_aqui_carrinho_api.Exceptions;
using Newtonsoft.Json;

namespace Amg_ingressos_aqui_carrinho_api.Model
{
    public class MessageReturn
    {
        public MessageReturn()
        {
            Message = string.Empty;
            Data = string.Empty;
        }

        /// <summary>
        /// Mensagem de retorno
        /// </summary>
        [JsonPropertyName("message")]
        [JsonProperty("message")]
        public string Message { get; set; }

        /// <summary>
        /// Objeto de dados retornado
        /// </summary>
        [JsonPropertyName("data")]
        [JsonProperty("data")]
        public object Data { get; set; }

        internal T ToObject<T>()
        {
            return (T)this.Data;
        }
        internal List<T> ToListObject<T>()
        {
            return (List<T>)this.Data;
        }
        internal T JsonToModel<T>()
        {
            return JsonConvert.DeserializeObject<T>(this.Data.ToString() ?? string.Empty) ?? throw new ConvertException("Não foi possivel converter.");
        }
    }
}