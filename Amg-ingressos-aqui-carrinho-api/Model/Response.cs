using Amg_ingressos_aqui_carrinho_api.Exceptions;
using Newtonsoft.Json;

namespace Amg_ingressos_aqui_carrinho_api.Model
{
    public class Response
    {

        public Response()
        {
            Data = string.Empty;
            Message = string.Empty;

        }
        public string Data { get; set; }
        public string Message { get; set; }

        internal T JsonToModel<T>()
        {
            return JsonConvert.DeserializeObject<T>(this.Data) ?? throw new ConvertException("Não foi possivel converter.");
        }
        internal T MessageJsonToModel<T>()
        {
            return JsonConvert.DeserializeObject<T>(this.Message) ?? throw new ConvertException("Não foi possivel converter.");
        }
    }
}