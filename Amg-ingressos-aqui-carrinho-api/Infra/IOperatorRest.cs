using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amg_ingressos_aqui_carrinho_api.Model;

namespace Amg_ingressos_aqui_carrinho_api.Infra
{
    public interface IOperatorRest 
    {
        Response SendRequestAsync(Request transactionJson, string urlServer, string Token);
    }
}