using System.ComponentModel.DataAnnotations;
using Amg_ingressos_aqui_carrinho_api.Enum;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Amg_ingressos_aqui_carrinho_api.Model
{
    public class Transaction
    {
        /// <summary>
        /// Id mongo
        /// </summary>
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        /// <summary>
        /// Id Usuario
        /// </summary>
        [BsonRepresentation(BsonType.ObjectId)]
        public string IdPerson { get; set; }

        /// <summary>
        /// Id Metodo Pagamento
        /// </summary>
        [BsonRepresentation(BsonType.ObjectId)]
        public string IdPaymentMethod { get; set; }

        /// <summary>
        /// Taxa de Compra
        /// </summary>
        public decimal Tax { get; set; }

        /// <summary>
        /// Lista de itens de pedido
        /// </summary>
        [BsonIgnore]
        public List<TransactionIten> TransactionItens { get; set; }

        /// <summary>
        /// Status Transacao
        /// </summary>
        public StatusPayment Status { get; set; }
        
    }
}