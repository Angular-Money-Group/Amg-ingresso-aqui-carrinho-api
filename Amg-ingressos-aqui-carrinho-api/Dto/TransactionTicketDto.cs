namespace Amg_ingressos_aqui_carrinho_api.Dto
{
    public class TransactionTicketDto
    {
        public string NameUser { get; set; }
        public string NameEvent { get; set; }
        public int CountTickets { get; set; }
        public string PaymentMethod { get; set; }
        public string PurchaseDate { get; set; }
        public string PurchaseTime { get; set; }
        public decimal Tax { get; set; }
        public decimal SubTotal { get; set; }
        public decimal Total { get; set; }
        public List<TicketDto> ListTickets { get; set; }
    }
    public class TicketDto {
        public string NameVariant { get; set; }
        public string QrCodeLink { get; set; }
    }
}