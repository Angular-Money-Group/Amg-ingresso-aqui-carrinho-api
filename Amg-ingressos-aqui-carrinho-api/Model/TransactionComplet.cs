using MongoDB.Bson.Serialization.Attributes;

namespace Amg_ingressos_aqui_carrinho_api.Model
{
    [BsonIgnoreExtraElements]
    public class TransactionComplet : Transaction
    {
        public TransactionComplet()
        {
            Events = new List<Event>();
            TransactionItens = new List<TransactionIten>();
            Tickets = new List<Ticket>();
        }

        public List<Event> Events { get; set; }
        public List<Ticket> Tickets { get; set; }
        public List<TransactionIten> TransactionItens { get; set; }
    }
}