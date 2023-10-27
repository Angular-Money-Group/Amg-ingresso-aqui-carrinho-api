namespace Amg_ingressos_aqui_carrinho_api.Infra
{
    public class PagBankSettings : TransactionSettings
    {
        /// <summary>
        /// Connection string base de dados Mongo
        /// </summary>
        public string ClientIdHomolog { get; set; } = null!;
        /// <summary>
        /// Nome base de dados Mongo
        /// </summary>
        public string ClientSecretHomolog { get; set; } = null!;
        /// <summary>
        /// Connection string base de dados Mongo
        /// </summary>
        public string TokenHomolog { get; set; } = null!;
    }
}