using Amg_ingressos_aqui_carrinho_api.Enum;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Amg_ingressos_aqui_carrinho_api.Model
{
    public class TransactionPayment
    {

        public TransactionPayment()
        {
            Id = string.Empty;
            IdTransaction = string.Empty;
            IdPaymentMethod = string.Empty;
        }

        /// <summary>
        /// Id mongo
        /// </summary>
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        /// <summary>
        /// Id Variante
        /// </summary>
        [BsonRepresentation(BsonType.ObjectId)]
        public string IdTransaction { get; set; }
        /// <summary>
        /// Status
        /// </summary>
        public StatusPayment Status { get; set; }
        /// <summary>
        /// valor total
        /// </summary>
        public decimal TotalValue { get; set; }

        /// <summary>
        /// valor total
        /// </summary>
        [BsonRepresentation(BsonType.ObjectId)]
        public string IdPaymentMethod { get; set; }

    }
}