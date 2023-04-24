using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Amg_ingressos_aqui_carrinho_api.Model
{
    public class TransactionIten
    {
        /// <summary>
        /// Id mongo
        /// </summary>
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

         /// <summary>
        /// Id Transaction
        /// </summary>
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string IdTransaction { get; set; }

        /// <summary>
        /// Id Variante
        /// </summary>
        [BsonRepresentation(BsonType.ObjectId)]
        public string IdVariante { get; set; }

        /// <summary>
        /// Id Lote
        /// </summary>
        [BsonRepresentation(BsonType.ObjectId)]
        public string IdLote { get; set; }

        /// <summary>
        /// Meia entrada
        /// </summary>
        public bool HalfPrice { get; set; }

        /// <summary>
        /// Preco Ingresso
        /// </summary>
        public decimal TicketPrice { get; set; }
        /// <summary>
        /// Quantidade de ingressos
        /// </summary>
        public decimal AmoutTicket { get; set; }
        /// <summary>
        /// Taxa de Compra
        /// </summary>
        public decimal Tax { get; set; }

    }
}