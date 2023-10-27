namespace Amg_ingressos_aqui_carrinho_api.Infra
{
    public class CieloSettings : TransactionSettings
    {
        /// <summary>
        /// Connection string base de dados Mongo
        /// </summary>
        public string MerchantIdHomolog { get; set; } = null!;
        /// <summary>
        /// Nome base de dados Mongo
        /// </summary>
        public string MerchantKeyHomolog { get; set; } = null!;
    }
}