using System.Text.Json.Serialization;

namespace Amg_ingressos_aqui_carrinho_api.Model.Pagbank
{
    public class Card
    {
        [JsonIgnore]
        public string number { get; set; }
        [JsonIgnore]
        public string exp_month { get; set; }
        [JsonIgnore]
        public string exp_year { get; set; }
        public string security_code { get; set; }
        public Holder holder { get; set; }
        public bool store { get; set; }
        public string encrypted { get; set; }
    }
}