namespace Amg_ingressos_aqui_carrinho_api.Model
{
    public class TransactionComplet : Transaction
    {
        public TransactionComplet()
        {
            Events = new List<Event>();
            Tickets= new List<Ticket>();
        }
        public List<Event> Events { get; set; }
        public List<Ticket> Tickets { get; set; }
    }
}