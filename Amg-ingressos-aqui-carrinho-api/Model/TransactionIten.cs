using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Amg_ingressos_aqui_carrinho_api.Model
{
    [BsonIgnoreExtraElements]
    public class TransactionIten
    {
        public TransactionIten()
        {
            Id = string.Empty;
            IdTransaction = string.Empty;
            IdTicket = string.Empty;
            Details = string.Empty;
        }

        /// <summary>
        /// Id mongo
        /// </summary>
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        /// <summary>
        /// Id Transaction
        /// </summary>
        [BsonRepresentation(BsonType.ObjectId)]
        public string IdTransaction { get; set; }

        /// <summary>
        /// Id Ticket
        /// </summary>
        [BsonRepresentation(BsonType.ObjectId)]
        public string IdTicket { get; set; }

        /// <summary>
        /// Meia entrada
        /// </summary>
        public bool HalfPrice { get; set; }

        /// <summary>
        /// Preco Ingresso
        /// </summary>
        public decimal TicketPrice { get; set; }

        /// <summary>
        /// Preco Ingresso
        /// </summary>
        public string Details { get; set; }

    }
}