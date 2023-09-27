using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Amg_ingressos_aqui_carrinho_api.Dto
{
    public class EventDto
    {
        public string name { get; set; }
        public string local { get; set; }
        public string type { get; set; }
        public string image { get; set; }
        public string description { get; set; }
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
        public Enum.StatusEvent status { get; set; }
        public Model.Address address { get; set; }
        //public List<TransactionEventDto> Transactions { get; set; }
    }
}