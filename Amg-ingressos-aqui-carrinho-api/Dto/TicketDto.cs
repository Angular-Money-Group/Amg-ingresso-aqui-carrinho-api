namespace Amg_ingressos_aqui_carrinho_api.Dto
{
    public class TicketDto
    {
        public TicketDto()
        {
            NameVariant = string.Empty;
            QrCodeLink = string.Empty;
        }
        public string NameVariant { get; set; }
        public string QrCodeLink { get; set; }
    }
}