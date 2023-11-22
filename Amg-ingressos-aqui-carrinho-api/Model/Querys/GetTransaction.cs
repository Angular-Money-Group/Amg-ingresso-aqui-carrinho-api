using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amg_ingressos_aqui_carrinho_api.Enum;
using Amg_ingressos_aqui_carrinho_api.Model;
using Amg_ingressos_aqui_carrinho_api.Model.Cielo.Callback;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Amg_ingressos_aqui_carrinho_api.Model.Querys
{
    public class GetTransactionEventData
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string _id { get; set; }
        [BsonRepresentation(BsonType.ObjectId)]
        public string IdPerson { get; set; }
        [BsonRepresentation(BsonType.ObjectId)]
        public string IdEvent { get; set; }
        public Event? Event { get; set; }
        public decimal Tax { get; set; }
        public decimal Discount { get; set; }
        public StatusPaymentEnum Status { get; set; }
        public StageTransactionEnum Stage { get; set; }
        public PaymentMethod? PaymentMethod { get; set; }
        public PaymentMethodPix? PaymentMethodPix { get; set; }
        public CallbackPix? PaymentPix { get; set; }
        public List<TransactionItens> TransactionIten { get; set; }
        public string? PaymentIdService { get; set; }
        public Decimal TotalValue { get; set; }
        public string? ReturnUrl { get; set; }
        public string? Details { get; set; }
        public int TotalTicket { get; set; }
        public DateTime DateRegister { get; set; }
    }

    public class TransactionItens
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string _id { get; set; }
        [BsonRepresentation(BsonType.ObjectId)]
        public string IdTransaction { get; set; }
        [BsonRepresentation(BsonType.ObjectId)]
        public string IdTicket { get; set; }
        public bool HalfPrice { get; set; }
        public string TicketPrice { get; set; }
        public string Details { get; set; }
        public List<Ticket> ticket { get; set; }
    }

    public class Ticket
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string _id { get; set; }
        [BsonRepresentation(BsonType.ObjectId)]
        public string IdLot { get; set; }
        [BsonRepresentation(BsonType.ObjectId)]
        public string IdUser { get; set; }
        public object Position { get; set; }
        public string Value { get; set; }
        public bool isSold { get; set; }
        public bool ReqDocs { get; set; }
        public string QrCode { get; set; }
        [BsonRepresentation(BsonType.ObjectId)]
        public string IdColab { get; set; }
        public object Status { get; set; }
        public Boolean ticketCortesia { get; set; }
    }
}