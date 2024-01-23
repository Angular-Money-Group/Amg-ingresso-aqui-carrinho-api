namespace Amg_ingressos_aqui_carrinho_api.Model.Cielo
{
    public class CardOnFile
    {
        public CardOnFile()
        {
            Usage = "First";
            Reason = "Recurring";
        }

        public string Usage { get; set; }
        public string Reason { get; set; }
    }
}