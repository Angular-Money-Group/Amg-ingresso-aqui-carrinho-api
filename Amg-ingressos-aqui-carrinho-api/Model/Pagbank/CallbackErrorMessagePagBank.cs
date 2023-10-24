using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Amg_ingressos_aqui_carrinho_api.Model.Pagbank
{
    public class CallbackErrorMessagePagBank
    {
        public List<ErrorMessage> Error_messages { get; set; }
    }
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class ErrorMessage
    {
        public string code { get; set; }
        public string description { get; set; }
        public string parameter_name { get; set; }
    }

}