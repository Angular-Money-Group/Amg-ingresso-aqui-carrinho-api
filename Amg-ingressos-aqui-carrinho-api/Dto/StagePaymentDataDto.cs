using Amg_ingressos_aqui_carrinho_api.Model;

namespace Amg_ingressos_aqui_carrinho_api.Dto
{
    public class StagePaymentDataDto : PaymentMethod
    {
        public StagePaymentDataDto()
        {
            Id = string.Empty;
        }
        public string Id { get; set; }
    }
}