using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amg_ingressos_aqui_carrinho_api.Enum;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Amg_ingressos_aqui_carrinho_api.Dto
{
    public class TransactionDto
    {
        /// <summary>
        /// Id Usuario
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Id Usuario
        /// </summary>
        public string IdUser { get; set; }

        
        /// <summary>
        /// TransactionItens
        /// </summary>
        public List<TransactionItenDto> TransactionItensDto { get; set; }
    }
}