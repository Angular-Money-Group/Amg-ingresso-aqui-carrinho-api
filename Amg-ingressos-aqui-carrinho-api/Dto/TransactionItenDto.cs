using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Amg_ingressos_aqui_carrinho_api.Dto
{
    public class TransactionItenDto
    {
        /// <summary>
        /// Id Ticket
        /// </summary>
        [BsonRepresentation(BsonType.ObjectId)]
        public string IdLot { get; set; }

        /// <summary>
        /// Quantidade ingressos
        /// </summary>
        public int AmountTicket { get; set; }

        /// <summary>
        /// meia entrada
        /// </summary>
        public bool HalfPrice { get; set; }
    }
}