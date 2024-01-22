using Newtonsoft.Json;

namespace Amg_ingressos_aqui_carrinho_api.Model
{
    public class Response
    {
        public string Data { get; set;}
        public string Message { get; set;} 

        internal T JsonToModel<T>()
        {
           return JsonConvert.DeserializeObject<T>(this.Data);
        }
        internal T MessageJsonToModel<T>()
        {
           return JsonConvert.DeserializeObject<T>(this.Message);
        }
    }
}