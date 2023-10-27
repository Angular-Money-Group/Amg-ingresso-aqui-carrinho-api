using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Amg_ingressos_aqui_carrinho_api.Model.Pagbank
{
    public class Boleto
    {
        public string due_date { get; set; }
        public InstructionLines instruction_lines { get; set; }
        public Holder holder { get; set; }
    }
}