namespace Amg_ingressos_aqui_carrinho_api.Services.Interfaces
{
    public interface IQrCodeService
    {
        Task<string> GenerateQrCode(string idTicket);
    }
}