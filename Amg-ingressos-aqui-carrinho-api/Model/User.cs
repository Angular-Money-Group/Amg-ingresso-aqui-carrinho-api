﻿using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;
using Amg_ingressos_aqui_carrinho_api.Enum;

namespace Amg_ingressos_aqui_carrinho_api.Model {
    public class User 
    {
        /// <summary>
        /// Id do usuário
        /// </summary>
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [BsonElement("_id")]
        public string? Id { get; set; }
        /// <summary>
        /// name
        /// </summary>
        [Required]
        [BsonElement("Name")]
        [JsonPropertyName("name")]
        public string? Name { get; set; }

        /// <sumary>
        /// Documento identificação
        /// </sumary>
        [Required]
        [BsonElement("DocumentId")]
        [JsonPropertyName("documentId")]
        public string? DocumentId { get; set; }

        /// <sumary>
        /// Status
        /// </sumary>
        [BsonElement("Status")]
        [JsonPropertyName("status")]
        public TypeStatusEnum? Status { get; set; }

        /// <summary>
        /// Tipo do usuário
        /// </summary>
        [Required]
        [BsonElement("Type")]
        [JsonPropertyName("type")]
        public TypeUserEnum? Type { get; set; }

        /// <summary>
        /// Endereço do usuário
        /// </summary>
        [BsonElement("Address")]
        [JsonPropertyName("address")]
        public Address? Address { get; set; }

        /// <summary>
        /// Contato do usuário
        /// </summary>
        [Required]
        [BsonElement("Contact")]
        [JsonPropertyName("contact")]
        public Contact? Contact { get; set; }

        /// <summary>
        /// Confirmação do usuário
        /// </summary>
        [BsonElement("UserConfirmation")]
        [JsonPropertyName("userConfirmation")]
        public UserConfirmation? UserConfirmation { get; set; }

        /// <summary>
        /// Senha de acesso
        /// </summary>
        [Required]
        [BsonElement("Password")]
        [JsonPropertyName("password")]
        public string? Password { get; set; }

        /// <summary>
        /// Senha de acesso
        /// </summary>
        [BsonElement("idAssociate")]
        [JsonPropertyName("idAssociate")]
        public string? IdAssociate { get; set; }
    }
}