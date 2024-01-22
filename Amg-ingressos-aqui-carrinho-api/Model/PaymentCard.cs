namespace Amg_ingressos_aqui_carrinho_api.Model
{
    public class PaymentCard
    {
        public PaymentCard()
        {
            Number = string.Empty;
            Exp_month = string.Empty;
            Exp_year = string.Empty;
            Security_code = string.Empty;
            Holder = new Holder();
        }

        public string Number { get; set; }
        public string Exp_month { get; set; }
        public string Exp_year { get; set; }
        public string Security_code { get; set; }
        public Holder Holder { get; set; }

    }
}