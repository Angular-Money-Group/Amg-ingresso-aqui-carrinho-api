
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
        public string Message;
        
        /// <summary>
        /// Objeto de dados retornado
        /// </summary>
        public object Data;

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
           return JsonConvert.DeserializeObject<T>(this.Data.ToString());
        }
    }
}