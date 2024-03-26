using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Amg_ingressos_aqui_carrinho_api.Model.Pagbank
{
    public class ErrorMessage
    {
        public ErrorMessage()
        {
            Code = string.Empty;
            Description = string.Empty;
            ParameterName = string.Empty;
        }

        [JsonProperty("code")]
        [JsonPropertyName("code")]
        public string Code { get; set; }

        [JsonProperty("description")]
        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonProperty("parameter_name")]
        [JsonPropertyName("parameter_name")]
        public string ParameterName { get; set; }
    }
}