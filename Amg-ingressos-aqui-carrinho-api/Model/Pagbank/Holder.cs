using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Amg_ingressos_aqui_carrinho_api.Model.Pagbank
{
    public class Holder
    {
        public string name { get; set; }
        [JsonIgnore]
        public string tax_id { get; set; }
        [JsonIgnore]
        public string email { get; set; }
        [JsonIgnore]
        public Address address { get; set; }
    }
}