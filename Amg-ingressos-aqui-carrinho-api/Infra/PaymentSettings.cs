namespace Amg_ingressos_aqui_carrinho_api.Infra
{
    public class PaymentSettings
    {
   
        /// <summary>
        /// Chave identificadora de gateway de pagamento
        /// </summary>
        public string Key { get; set; } = null!;
        public CieloSettings CieloSettings { get; set; } = null!;
        public PagBankSettings PagBankSettings { get; set; } = null!;
    }
}