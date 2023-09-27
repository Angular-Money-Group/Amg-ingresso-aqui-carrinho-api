using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amg_ingressos_aqui_carrinho_api.Enum;
using Amg_ingressos_aqui_carrinho_api.Model;

namespace Amg_ingressos_aqui_carrinho_api.Dto
{
    public class TransactionEventDto
    {
        /// <summary>
        /// Id mongo
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Id Usuario
        /// </summary>
        public string IdPerson { get; set; }

        /// <summary>
        /// Id Usuario
        /// </summary>
        public string IdEvent { get; set; }

        /// <summary>
        /// Meio de pagamento
        /// </summary>

        public PaymentMethod PaymentMethod { get; set; }

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
        public List<TransactionIten> TransactionItens { get; set; }

        
        /// <summary>
        /// Status Transacao
        /// </summary>
        public StatusPaymentEnum Status { get; set; }
        
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