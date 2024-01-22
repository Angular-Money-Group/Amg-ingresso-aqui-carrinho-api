using System.ComponentModel.DataAnnotations;
using Amg_ingressos_aqui_carrinho_api.Enum;
using Amg_ingressos_aqui_carrinho_api.Model.Pagbank;

namespace Amg_ingressos_aqui_carrinho_api.Model
{
    public class PaymentMethod
    {
        public PaymentMethod()
        {
            IdPaymentMethod = string.Empty;
            CardNumber = string.Empty;
            Holder = string.Empty;
            ExpirationDate = string.Empty;
            SecurityCode = string.Empty;
            Brand = string.Empty;
            EncryptedCard = string.Empty;
            AuthenticationMethod = new AuthenticationMethod();
        }

        /// <summary>
        /// Id Metodo Pagamento
        /// </summary>
        public string? IdPaymentMethod { get; set; }

        /// <summary>
        /// Tipo de pagamento
        /// </summary>
        [Required]
        public TypePayment TypePayment { get; set; }

        /// <summary>
        /// Numero cartao
        /// </summary>
        public string? CardNumber { get; set; }

        /// <summary>
        /// Nome no Cartao
        /// </summary>
        public string? Holder { get; set; }

        /// <summary>
        /// Data Expiracao
        /// </summary>
        public string? ExpirationDate { get; set; }

        /// <summary>
        /// Codigo seguranca
        /// </summary>
        public string? SecurityCode { get; set; }

        /// <summary>
        /// Salvar Cartao
        /// </summary>
        public bool? SaveCard { get; set; }

        /// <summary>
        /// Bandeira
        /// </summary>
        public string? Brand { get; set; }

        /// <summary>
        /// Numero de parcelas
        /// </summary>
        public int? Installments { get; set; }
        /// <summary>
        /// cartao criptografado pagbank
        /// </summary>
        public string EncryptedCard { get; set; }

        public AuthenticationMethod AuthenticationMethod { get; set; }

        public Transaction ToModelTransaction(){
            return new Transaction()
            {
                PaymentMethod = new PaymentMethod()
                {
                    IdPaymentMethod = this.IdPaymentMethod,
                    Brand = this.Brand,
                    CardNumber = this.CardNumber,
                    ExpirationDate = this.ExpirationDate,
                    Holder = this.Holder,
                    SaveCard = this.SaveCard,
                    SecurityCode = this.SecurityCode,
                    TypePayment = this.TypePayment,
                    Installments = this.Installments,
                    EncryptedCard = this.EncryptedCard,
                    AuthenticationMethod = this.AuthenticationMethod
                },
                Stage = StageTransaction.PaymentData
            };
        }
    }
}
