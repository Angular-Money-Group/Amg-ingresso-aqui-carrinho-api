using Amg_ingressos_aqui_carrinho_api.Model;

namespace Amg_ingressos_aqui_carrinho_api.Dto
{
    public class StageTicketDataDto
    {
        public decimal Tax { get; set; }
        public decimal Discount { get; set; }

        public Transaction DtoToModel()
        {
            return new Transaction()
            {
                Tax = this.Tax,
                Discount = this.Discount,
                Stage = Enum.StageTransaction.TicketsData
            };
        }
    }
}