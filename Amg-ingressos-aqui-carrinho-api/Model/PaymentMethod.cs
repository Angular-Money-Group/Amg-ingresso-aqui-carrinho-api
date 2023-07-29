using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Amg_ingressos_aqui_carrinho_api.Enum;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Amg_ingressos_aqui_carrinho_api.Model
{
    public class PaymentMethod
    {
        /// <summary>
        /// Id Metodo Pagamento
        /// </summary>
        public string? IdPaymentMethod { get; set; }
        /// <summary>
        /// Tipo de pagamento
        /// </summary>
        [Required]
        public TypePaymentEnum TypePayment { get; set; }
        /// <summary>
        /// Numero cartao
        /// </summary>
        public string CardNumber { get; set; }
        /// <summary>
        /// Nome no Cartao
        /// </summary>
        public string Holder { get; set; }
        /// <summary>
        /// Data Expiracao
        /// </summary>
        public string ExpirationDate { get; set; }
        /// <summary>
        /// Codigo seguranca
        /// </summary>
        public string SecurityCode { get; set; }
        /// <summary>
        /// Salvar Cartao 
        /// </summary>
        public bool SaveCard { get; set; }
        /// <summary>
        /// Bandeira
        /// </summary>
        public string Brand { get; set; }
        /// <summary>
        /// Numero de parcelas
        /// </summary>
        public int Installments { get; set; }
        
        
        
    }
}