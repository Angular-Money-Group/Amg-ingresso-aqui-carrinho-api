using System.ComponentModel.DataAnnotations;
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
        /// Id Variante
        /// </summary>
        [BsonRepresentation(BsonType.ObjectId)]
        public string IdEvent { get; set; }

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
        /// Lista de itens de pedido
        /// </summary>
        public List<TransactionIten> TransactionItens { get; set; }
        
    }
}