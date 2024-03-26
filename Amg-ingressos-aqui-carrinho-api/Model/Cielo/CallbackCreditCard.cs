namespace Amg_ingressos_aqui_carrinho_api.Model.Cielo.Callback
{
    public class CallbackCreditCard
    {
        public CallbackCreditCard()
        {
            MerchantOrderId = string.Empty;
            Payment = new PaymentCallbackCreditCard();
            Customer = new Customer();
        }

        public string MerchantOrderId { get; set; }
        public Customer Customer { get; set; }
        public PaymentCallbackCreditCard Payment { get; set; }
    }
}