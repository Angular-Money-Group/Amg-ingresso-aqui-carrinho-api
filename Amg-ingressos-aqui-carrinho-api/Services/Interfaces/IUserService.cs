using Amg_ingressos_aqui_carrinho_api.Model;

namespace Amg_ingressos_aqui_carrinho_api.Services.Interfaces
{
    public interface IUserService
    {
        Task<MessageReturn> FindByIdAsync(string idUser);
    }
}