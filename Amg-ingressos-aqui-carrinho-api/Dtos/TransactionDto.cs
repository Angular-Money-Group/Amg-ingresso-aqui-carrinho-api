using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Amg_ingressos_aqui_carrinho_api.Dtos
{
    public class TransactionDto
    {
        /// <summary>
        /// Id Usuario
        /// </summary>
        [BsonRepresentation(BsonType.ObjectId)]
        public string IdCustomer { get; set; }
        
        /// <summary>
        /// TransactionItens
        /// </summary>
        public List<TransactionItenDto> TransactionItensDto { get; set; }
    }
}