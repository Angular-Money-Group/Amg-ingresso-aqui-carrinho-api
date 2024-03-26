namespace Amg_ingressos_aqui_carrinho_api.Model.Cielo
{
    public class Link
    {
        public Link()
        {
            Method = string.Empty;
            Rel = string.Empty;
            Href = string.Empty;
        }

        public string Method { get; set; }
        public string Rel { get; set; }
        public string Href { get; set; }
    }
}