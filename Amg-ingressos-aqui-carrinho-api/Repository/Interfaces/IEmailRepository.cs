using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amg_ingressos_aqui_carrinho_api.Model;

namespace Amg_ingressos_aqui_carrinho_api.Repository.Interfaces
{
    public interface IEmailRepository
    {
        public Task<object> SaveAsync(object email);
    }
}