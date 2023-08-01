using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace Amg_ingressos_aqui_carrinho_api.Model
{
    public class Ticket
    {
        /// <summary>
        /// Id mongo
        /// </summary>
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [JsonProperty("id")]
        public string? Id { get; set; }
        
        /// <summary>
        /// Id mongo Lote
        /// </summary>
        [JsonProperty("idLot")]
        public string? IdLot { get; set; }

        /// <summary>
        /// Id mongo Usuário
        /// </summary>
        [JsonProperty("idUser")]
        public string? IdUser { get; set; }
        
        /// <summary>
        /// Posicao
        /// </summary>
        [JsonProperty("position")]
        public string? Position { get; set; }

        /// <summary>
        /// Valor Ingresso
        /// </summary>
        [JsonProperty("value")]
        public decimal Value { get; set; }

        /// <summary>
        /// Se o ingresso já foi vendido
        /// </summary>
        [BsonDefaultValue(false)]
        [JsonProperty("isSold")]
        public bool isSold { get; set; }
        /// <summary>
        /// QrCode
        /// </summary>
        [JsonProperty("qrCode")]
        public string QrCode { get; set; }
    }
}