using Amg_ingressos_aqui_carrinho_api.Enum;
using Amg_ingressos_aqui_carrinho_api.Model.Cielo.Callback;
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
        /// Id Usuario
        /// </summary>
        [BsonRepresentation(BsonType.ObjectId)]
        public string IdEvent { get; set; }

        /// <summary>
        /// Meio de pagamento
        /// </summary>

        public PaymentMethod PaymentMethod { get; set; }

        /// <summary>
        /// Meio de pagamento
        /// </summary>

        public PaymentMethodPix PaymentMethodPix { get; set; }

        /// <summary>
        /// Meio de pagamento
        /// </summary>

        public CallbackPix? PaymentPix { get; set; }

        /// <summary>
        /// Taxa de Compra
        /// </summary>
        public decimal Tax { get; set; }

        /// <summary>
        /// Valor total sem desconto ou taxas
        /// </summary>
        public decimal TotalValue { get; set; }

        /// <summary>
        /// Desconto
        /// </summary>
        public decimal Discount { get; set; }

        /// <summary>
        /// Lista de itens de pedido
        /// </summary>
        [BsonIgnore]
        public List<TransactionIten> TransactionItens { get; set; }

        /// <summary>
        /// Status Transacao
        /// </summary>
        public StatusPaymentEnum Status { get; set; }

        /// <summary>
        /// Etapa de transacao
        /// </summary>
        public StageTransactionEnum Stage { get; set; }

        /// <summary>
        /// Url de Retorno Transacao
        /// </summary>
        public string ReturnUrl { get; set; }

        /// <summary>
        /// PaymentId Cielo
        /// </summary>
        public string PaymentIdService { get; set; }

        /// <summary>
        /// Detalhes de transacao
        /// </summary>
        public string Details { get; set; }

        /// <summary>
        /// Total Ticket
        /// </summary>
        public int TotalTicket { get; set; }

        /// <summary>
        /// Data Cadastro
        /// </summary>
        public DateTime DateRegister { get; set; }
    }

    public class PaymentPixDetails
    {
        public string Paymentid { get; set; }
        public string Type { get; set; }
        public string AcquirerTransactionId { get; set; }
        public string ProofOfSale { get; set; }
        public string QrcodeBase64Image { get; set; }
        public string QrCodeString { get; set; }
        public long Amount { get; set; }
        public string ReceivedDate { get; set; }
        public int Status { get; set; }
        public StatusCallbackCielo ReturnCode { get; set; }
    }

    public class TransactionPix
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
        /// Id Usuario
        /// </summary>
        [BsonRepresentation(BsonType.ObjectId)]
        public string IdEvent { get; set; }

        /// <summary>
        /// Meio de pagamento
        /// </summary>

        public PaymentMethodPix PaymentMethod { get; set; }

        /// <summary>
        /// Taxa de Compra
        /// </summary>
        public decimal Tax { get; set; }

        /// <summary>
        /// Valor total sem desconto ou taxas
        /// </summary>
        public decimal TotalValue { get; set; }

        /// <summary>
        /// Desconto
        /// </summary>
        public decimal Discount { get; set; }

        /// <summary>
        /// Lista de itens de pedido
        /// </summary>
        [BsonIgnore]
        public List<TransactionIten> TransactionItens { get; set; }

        /// <summary>
        /// Status Transacao
        /// </summary>
        public StatusPaymentEnum Status { get; set; }

        /// <summary>
        /// Etapa de transacao
        /// </summary>
        public StageTransactionEnum Stage { get; set; }

        /// <summary>
        /// Url de Retorno Transacao
        /// </summary>
        public string ReturnUrl { get; set; }

        /// <summary>
        /// PaymentId Cielo
        /// </summary>
        public string PaymentIdService { get; set; }

        /// <summary>
        /// Detalhes de transacao
        /// </summary>
        public string Details { get; set; }

        /// <summary>
        /// Total Ticket
        /// </summary>
        public int TotalTicket { get; set; }

        /// <summary>
        /// Data Cadastro
        /// </summary>
        public DateTime DateRegister { get; set; }
    }
}
