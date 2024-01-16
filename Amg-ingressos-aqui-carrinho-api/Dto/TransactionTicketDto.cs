using Amg_ingressos_aqui_carrinho_api.Model;

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

        public List<TransactionTicketDto> ListModelToListDto(List<TransactionComplet> transactions){
            return transactions.Select(t=> ModelToDto(t)).ToList();
        }

        public TransactionTicketDto ModelToDto(TransactionComplet transaction)
        {
            return new TransactionTicketDto()
            {
                CountTickets = transaction.TotalTicket,
                NameUser = transaction.IdPerson,
                NameEvent = transaction.Events.Find(e => e._Id == transaction.IdEvent).Name,
                PaymentMethod = transaction?.PaymentMethod?.TypePayment.ToString() ?? string.Empty,
                PurchaseDate = transaction.DateRegister.ToLocalTime().ToString("dd-MM-yyyy"),
                PurchaseTime = transaction.DateRegister.ToLocalTime().ToString("hh:mm:ss"),
                SubTotal = transaction.TotalValue,
                Tax = transaction.Tax,
                Total = (transaction.TotalValue + transaction.Tax) - transaction.Discount,
                ListTickets = transaction.TransactionItens.Select(x =>
                    new TicketDto()
                    {
                        NameVariant = x.Details,
                        QrCodeLink = transaction.Tickets?.Find(t => t.Id == x.IdTicket)?.QrCode ?? string.Empty
                    }
                ).ToList()
            };
        }
    }
}