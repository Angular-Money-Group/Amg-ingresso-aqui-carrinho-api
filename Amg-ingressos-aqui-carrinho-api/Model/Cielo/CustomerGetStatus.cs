namespace Amg_ingressos_aqui_carrinho_api.Model.Cielo
{
    public class CustomerGetStatus
    {
        public CustomerGetStatus()
        {
            Name = string.Empty;
            Identity = string.Empty;
            Address = new Dictionary<string, string>();
        }

        public string Name { get; set; }
        public string Identity { get; set; }
        public Dictionary<string, string> Address { get; set; }
    }
}